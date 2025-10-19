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
        private string _currentPrinterFilter = "All Printers";
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
            };


            _uiRefreshTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiRefreshTimer.Tick += UiRefreshTimer_Tick;
        }

        private void OnPrinterFilterChanged(string selectedPrinter)
        {
            _currentPrinterFilter = selectedPrinter;
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            List<PrintJobGroup> filteredGroups;
            lock (_lock)
            {
                var allGroups = _jobGroups.Values.ToList();
                if (string.IsNullOrEmpty(_currentPrinterFilter) || _currentPrinterFilter == "All Printers")
                {
                    filteredGroups = allGroups; 
                }
                else
                {
                    
                    filteredGroups = allGroups
                        .Where(g => g.PrinterName.Equals(_currentPrinterFilter, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            _view.DisplayJobGroups(filteredGroups);
        }

        private void OnJobReceived(PrintJob job)
        {
            PrintJobGroup group = null;
            lock (_lock)
            {
                string baseDocName = GetBaseDocumentName(job.DocumentName);
                string groupKey = $"{job.User}|{baseDocName}".ToLower();
               

                if (_jobGroups.TryGetValue(groupKey, out var existingGroup) && existingGroup.GroupingStatus == "Grouping")
                {
                   
                    group = existingGroup;
                }
                else
                {
                   
                    group = new PrintJobGroup(groupKey);
                    _jobGroups[groupKey] = group;
                }

                var existingJobInGroup = group.Jobs.FirstOrDefault(j => j.JobId == job.JobId);
                if (existingJobInGroup != null)
                {
                  
                    group.Jobs.Remove(existingJobInGroup);
                    group.Jobs.Add(job);
                }
                else
                {
                  
                    group.Jobs.Add(job);
                }

                
                group.LastActivity = DateTime.Now;
                group.GroupingStatus = "Grouping";
            }

            CheckAndArchiveIfTerminal(group);
            ApplyFilters();
        }

        private void OnJobDeleted(PrintJob deletedJob)
        {
            PrintJobGroup group = null;
            lock (_lock)
            {
                string baseDocName = GetBaseDocumentName(deletedJob.DocumentName);
                string groupKey = $"{deletedJob.User}|{baseDocName}".ToLower();

                if (_jobGroups.TryGetValue(groupKey, out group))
                {
                    var jobInGroup = group.Jobs.FirstOrDefault(j => j.JobId == deletedJob.JobId);
                    if (jobInGroup != null)
                        jobInGroup.Status = "Completed";
                }
            }
            CheckAndArchiveIfTerminal(group);
            ApplyFilters();
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
        private void CheckAndArchiveIfTerminal(PrintJobGroup group)
        {
            if (group == null) return;

            bool isTerminal = group.Status == "Finished" ||
                              group.Status == "Error" ||
                              group.Status.Contains("Cancel");

            if (isTerminal)
            {
                ArchiveGroup(group);
                _jobGroups.Remove(group.GroupKey);
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
