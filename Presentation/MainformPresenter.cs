using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.UI.Interfaces;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Intrinsics.X86;
using Newtonsoft.Json;
using System.IO;

namespace PrintJobInterceptor.Presentation
{
    public class MainFormPresenter
    {
        private const int GROUPING_TIMEOUT_MS = 5000;
        private readonly IMainFormView _view;
        private readonly IPrintJobService _printJobService;

        private readonly Dictionary<string, PrintJobGroup> _jobGroups = new();
        private readonly object _lock = new();
        private readonly System.Windows.Forms.Timer _uiRefreshTimer;

        private List<PrintJobGroup> _allJobGroups = new List<PrintJobGroup>();
        private List<string> _currentPrinterFilter = new List<string>();
        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;

          
            _printJobService.JobSpooling += OnJobReceived;
            _printJobService.JobUpdated += OnJobReceived;
            _printJobService.JobDeleted += OnJobDeleted;

            _view.PrinterFilterChanged += OnPrinterFilterChanged;

            _view.PauseJobRequested += (group) => {
                if (group == null) return;
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.PauseJob(job.JobId, job.PrinterName);
                }
            };
            _view.ResumeJobRequested += (group) => {
                if (group == null) return;
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.ResumeJob(job.JobId, job.PrinterName);
                }
            };
            _view.CancelJobRequested += (group) => {
                if (group == null) return;
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.CancelJob(job.JobId, job.PrinterName);
                }
                ApplyFilters();
            };


            _uiRefreshTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiRefreshTimer.Tick += UiRefreshTimer_Tick;
        }

        private void OnPrinterFilterChanged(List<string> selectedPrinters)
        {
            _currentPrinterFilter = selectedPrinters ?? new List<string>();
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            List<PrintJobGroup> filteredGroups;
            lock (_lock)
            {
                var allGroups = _jobGroups.Values.ToList();
                if (_currentPrinterFilter.Count == 0)
                {
                    filteredGroups = allGroups;
                }
                else
                {

                    filteredGroups = allGroups
                            .Where(g => _currentPrinterFilter.Contains(g.PrinterName, StringComparer.OrdinalIgnoreCase))
                            .ToList();
                }
            }

            _view.DisplayJobGroups(filteredGroups);
        }

        private void OnJobReceived(PrintJob job)
        {
            lock (_lock)
            {

                var existingGroupForJob = _jobGroups.Values
                    .FirstOrDefault(g => g.Jobs.Any(j => j.JobId == job.JobId));

                if (existingGroupForJob != null)
                {

                    var jobToUpdate = existingGroupForJob.Jobs.First(j => j.JobId == job.JobId);


                    existingGroupForJob.Jobs.Remove(jobToUpdate);
                    existingGroupForJob.Jobs.Add(job);
                    existingGroupForJob.LastActivity = DateTime.Now;
                }
                else
                {

                    string baseDocName = GetBaseDocumentName(job.DocumentName);
                    string logicalGroupKey = $"{job.User}|{baseDocName}".ToLower();


                    var groupToAddTo = _jobGroups.Values.FirstOrDefault(g => g.GroupKey == logicalGroupKey && g.GroupingStatus == "Grouping");

                    if (groupToAddTo != null)
                    {

                        groupToAddTo.Jobs.Add(job);
                        groupToAddTo.LastActivity = DateTime.Now;
                    }
                    else
                    {

                        var newGroup = new PrintJobGroup(logicalGroupKey);
                        newGroup.Jobs.Add(job);
                        newGroup.LastActivity = DateTime.Now;
                        newGroup.GroupingStatus = "Grouping";

                        if (!_jobGroups.ContainsKey(logicalGroupKey))
                        {
                            _jobGroups.Add(logicalGroupKey, newGroup);
                        }

                    }
                }
            }


            ApplyFilters();
        }

        private void OnJobDeleted(PrintJob deletedJob)
        {
           
            var groupEntry = _jobGroups
            .FirstOrDefault(kvp => kvp.Value.Jobs.Any(j => j.JobId == deletedJob.JobId));

            if (groupEntry.Value == null)
            {
                ApplyFilters();
                return;
            }

            string dictionaryKey = groupEntry.Key; 
            PrintJobGroup group = groupEntry.Value;

            lock (_lock)
            {
                var jobInGroup = group.Jobs.FirstOrDefault(j => j.JobId == deletedJob.JobId);
                if (jobInGroup != null)
                {
                    
                    jobInGroup.Status = "Completed";
                    group.LastActivity = DateTime.Now;
                }

                
                CheckAndArchiveIfTerminal(dictionaryKey, group);
            }

            ApplyFilters();
        }

       
        private void CheckAndArchiveIfTerminal(string dictionaryKey, PrintJobGroup group)
        {
            if (group == null) return;

            bool isTerminal = group.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase) ||
                              group.Status.Contains("Error") ||
                              group.Status.Contains("Cancel");

            if (isTerminal)
            {
                ArchiveGroup(group);
              
                _jobGroups.Remove(dictionaryKey);
            }
        }

        private string GetBaseDocumentName(string documentName)
        {
          
            var match = Regex.Match(documentName, @"(.+?)_(\d+)(\..+)?$");
            if (match.Success)
                return match.Groups[1].Value + (match.Groups[3].Success ? match.Groups[3].Value : "");

            match = Regex.Match(documentName, @"(.+?)\s*\(Part\s*\d+(?:\s*of\s*\d+)?\)(\..+)?$");
            if (match.Success)
                return match.Groups[1].Value + (match.Groups[2].Success ? match.Groups[2].Value : "");

            return documentName;
        }
        

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            bool needsUiRefresh = false;


            lock (_lock)
            {
                
                var groupsToFinalize = _jobGroups.Values
                    .Where(g => g.GroupingStatus == "Grouping" &&
                                (DateTime.Now - g.LastActivity).TotalMilliseconds > GROUPING_TIMEOUT_MS)
                    .ToList();

                foreach (var g in groupsToFinalize)
                {
                    g.GroupingStatus = "Finalized";
                    needsUiRefresh = true;
                }

              
                var potentiallyStuckJobs = _jobGroups.Values
                    .SelectMany(g => g.Jobs)
                    .Where(j => (j.Status.Contains("Printing") || j.Status.Contains("Spooling"))
                             && (DateTime.Now - j.SubmittedAt).TotalSeconds > 15)
                    .ToList();

                foreach (var job in potentiallyStuckJobs)
                {
                    if (!_printJobService.DoesJobExist(job.JobId, job.PrinterName))
                    {
                        job.Status = "Completed";
                        needsUiRefresh = true;
                    }
                }
            }

            if (needsUiRefresh)
                ApplyFilters();
        }

        #region HistoryJson
        private void ArchiveGroup(PrintJobGroup group)
        {
            try
            {
               
                string historyDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "PrintJobInterceptor");

                Directory.CreateDirectory(historyDir); 
                string historyFile = Path.Combine(historyDir, "history.jsonl");

                string json = JsonConvert.SerializeObject(group);

                
                File.AppendAllText(historyFile, json + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to archive job group: {ex.Message}");
            }
        }
        
        #endregion
        public void Start()
        {
            _printJobService.StartMonitoring();
            _uiRefreshTimer.Start();
        }
        public void Stop()
        {
            _uiRefreshTimer.Stop();
            _printJobService.StopMonitoring();
        }
    }
}
