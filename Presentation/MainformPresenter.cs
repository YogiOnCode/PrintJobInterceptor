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
using Microsoft.Extensions.Logging;

namespace PrintJobInterceptor.Presentation
{
    public class MainFormPresenter
    {
        private const int GROUPING_TIMEOUT_MS = 5000;
        private IMainFormView _view;
        private readonly IPrintJobService _printJobService;


        private readonly Dictionary<string, PrintJobGroup> _jobGroups = new();
        private readonly object _lock = new();
        private readonly System.Windows.Forms.Timer _uiRefreshTimer;

        
        private List<string> _currentPrinterFilter = new List<string>();
        private readonly ILogger<MainFormPresenter> _logger;

        public MainFormPresenter(IPrintJobService printJobService, ILogger<MainFormPresenter> logger)
        {
            
            _printJobService = printJobService;
            _logger = logger;

            _printJobService.JobSpooling += OnJobReceived;
            _printJobService.JobUpdated += OnJobReceived;
            _printJobService.JobDeleted += OnJobDeleted;

            _uiRefreshTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiRefreshTimer.Tick += UiRefreshTimer_Tick;
        }

        public void SetView(IMainFormView view)
        {
            _view = view;


            _view.PrinterFilterChanged += OnPrinterFilterChanged;

            _view.PauseJobRequested += (group) => {
                if (group == null)
                {
                    _logger.LogWarning("PauseJobRequested fired but no group was selected.");
                    return;
                }
                _logger.LogInformation("Pause requested for group {GroupKey}. Pausing {JobCount} jobs.", group.GroupKey, group.Jobs.Count);
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.PauseJob(job.JobId, job.PrinterName);
                }
                ApplyFilters();
            };
            _view.ResumeJobRequested += (group) => {
                if (group == null)
                {
                    _logger.LogWarning("ResumeJobRequested fired but no group was selected.");
                    return;
                }
                _logger.LogInformation("Resume requested for group {GroupKey}. Resuming {JobCount} jobs.", group.GroupKey, group.Jobs.Count);
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.ResumeJob(job.JobId, job.PrinterName);
                }
                ApplyFilters();
            };
            _view.CancelJobRequested += (group) => {
                if (group == null)
                {
                    _logger.LogWarning("CancelJobRequested fired but no group was selected.");
                    return;
                }
                _logger.LogInformation("Cancel requested for group {GroupKey}. Cancelling {JobCount} jobs.", group.GroupKey, group.Jobs.Count);
                foreach (var job in group.Jobs.ToList())
                {
                    _printJobService.CancelJob(job.JobId, job.PrinterName);
                }
                ApplyFilters();
            };

        }

        private void OnPrinterFilterChanged(List<string> selectedPrinters)
        {
            _logger.LogInformation("Printer filter changed. {Count} printers selected.", selectedPrinters.Count);
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
            _logger.LogDebug("Applying filters. Displaying {Count} groups.", filteredGroups.Count);
            _view.DisplayJobGroups(filteredGroups);
        }

        private void OnJobReceived(PrintJob job)
        {
            _logger.LogInformation("Job received/updated. JobID: {JobID}, Status: {JobStatus}, Printer: {Printer}", job.JobId, job.Status, job.PrinterName);
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
                        _logger.LogDebug("Adding JobID {JobID} to existing grouping group {GroupKey}", job.JobId, groupToAddTo.GroupKey);
                        groupToAddTo.Jobs.Add(job);
                        groupToAddTo.LastActivity = DateTime.Now;
                    }
                    else
                    {
                        string dictionaryKey = Guid.NewGuid().ToString();
                        var newGroup = new PrintJobGroup(logicalGroupKey);
                        newGroup.Jobs.Add(job);
                        newGroup.LastActivity = DateTime.Now;
                        newGroup.GroupingStatus = "Grouping";

                        _jobGroups.Add(dictionaryKey, newGroup);

                    }
                }
            }


            ApplyFilters();
        }

        private void OnJobDeleted(PrintJob deletedJob)
        {
            _logger.LogInformation("Job deleted event received for JobID: {JobID}", deletedJob.JobId);
            var groupEntry = _jobGroups
            .FirstOrDefault(kvp => kvp.Value.Jobs.Any(j => j.JobId == deletedJob.JobId));

            if (groupEntry.Value == null)
            {
                _logger.LogWarning("JobDeleted event for JobID {JobID}, but no matching group found. Could be filter mismatch.", deletedJob.JobId);
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
                group.GroupingStatus = "Finalized";

                _logger.LogInformation("Group {GroupKey} has reached terminal status: {Status}. Archiving and removing.", dictionaryKey, group.Status);
              
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

                if (groupsToFinalize.Any())
                {
                    _logger.LogInformation("Finalizing {Count} job groups due to timeout.", groupsToFinalize.Count);
                    foreach (var g in groupsToFinalize)
                    {
                        g.GroupingStatus = "Finalized";
                        needsUiRefresh = true;
                    }
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
                        _logger.LogWarning("Job {JobID} on {Printer} appears stuck. Marking as 'Completed'.", job.JobId, job.PrinterName);
                        job.Status = "Completed";
                        needsUiRefresh = true;
                    }
                }
            }

            if (needsUiRefresh)
            {
                _logger.LogDebug("UIRefreshTimer triggered a UI refresh.");
                ApplyFilters();
            }
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
                _logger.LogInformation("Successfully archived group {GroupKey} to history file.", group.GroupKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to archive job group {GroupKey}", group.GroupKey);
            }
        }
        
        #endregion
        public void Start()
        {
            _logger.LogInformation("Presenter starting... Starting monitor and UI refresh timer.");
            _printJobService.StartMonitoring();
            _uiRefreshTimer.Start();
        }
        public void Stop()
        {
            _logger.LogInformation("Presenter stopping... Stopping monitor and UI refresh timer.");
            _uiRefreshTimer.Stop();
            _printJobService.StopMonitoring();
        }
    }
}
