using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.UI.Interfaces;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PrintJobInterceptor.Presentation
{
    public class MainFormPresenter
    {
        private const int GROUPING_TIMEOUT_MS = 5000; 
        private readonly IMainFormView _view;
        private readonly IPrintJobService _printJobService;

        private readonly Dictionary<string, PrintJobGroup> _jobGroups = new Dictionary<string, PrintJobGroup>();

        private readonly object _lock = new object();
        private readonly System.Windows.Forms.Timer _uiRefreshTimer;

        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;

        
            _printJobService.JobSpooling += OnJobReceived;
            _printJobService.JobUpdated += OnJobReceived;
            _printJobService.JobDeleted += OnJobDeleted;

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

        private void OnJobReceived(PrintJob job)
        {
            lock (_lock)
            {
                string baseDocName = GetBaseDocumentName(job.DocumentName);
                string groupKey = $"{job.User}|{baseDocName}".ToLower();

                if (_jobGroups.TryGetValue(groupKey, out var group) && group.GroupingStatus == "Grouping")
                {

                    var existingJob = group.Jobs.FirstOrDefault(j => j.JobId == job.JobId);
                    if (existingJob != null)
                    {
                        
                        group.Jobs.Remove(existingJob);
                        group.Jobs.Add(job);
                    }
                    else
                    {
                       
                        group.Jobs.Add(job);
                       
                    }
                    group.LastActivity = DateTime.Now;
                }
                else
                {
                    if (group != null)
                    {
                        groupKey = $"{groupKey}_{DateTime.Now:HHmmssfff}";
                    }

                    var newGroup = new PrintJobGroup(groupKey);
                    newGroup.Jobs.Add(job);
                    _jobGroups.Add(groupKey, newGroup);
                }
            }
        }
        private void OnJobDeleted(PrintJob deletedJob)
        {
            lock (_lock)
            {
                string baseDocName = GetBaseDocumentName(deletedJob.DocumentName);
                string groupKey = $"{deletedJob.User}|{baseDocName}".ToLower();

                if (_jobGroups.TryGetValue(groupKey, out var group))
                {
                    var jobInGroup = group.Jobs.FirstOrDefault(j => j.JobId == deletedJob.JobId);
                    if (jobInGroup != null)
                    {
                        if (deletedJob.Status != null &&
                            (deletedJob.Status.IndexOf("delet", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             deletedJob.Status.IndexOf("cancel", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            jobInGroup.Status = "Cancelled";
                        }
                        else
                        {
                            jobInGroup.Status = "Completed";
                        }
                    }
                }
            }
        }
        private string GetBaseDocumentName(string documentName)
        {
            
            var match = Regex.Match(documentName, @"(.+?)_(\d+)(\..+)?$");
            if (match.Success)
            {
                return match.Groups[1].Value + (match.Groups[3].Success ? match.Groups[3].Value : "");
            }

          
            match = Regex.Match(documentName, @"(.+?)\s*\(Part\s*\d+(?:\s*of\s*\d+)?\)(\..+)?$");
            if (match.Success)
            {
                return match.Groups[1].Value + (match.Groups[2].Success ? match.Groups[2].Value : "");
            }

            return documentName; 
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            bool needsUiRefresh = false;
            List<PrintJobGroup> groupsToFinalize;

            lock (_lock)
            {
                // ASSIGNED HERE: The list is assigned its value inside the lock.
                groupsToFinalize = _jobGroups.Values
                    .Where(g => g.GroupingStatus == "Grouping" && (DateTime.Now - g.LastActivity).TotalMilliseconds > GROUPING_TIMEOUT_MS)
                    .ToList();

                foreach (var group in groupsToFinalize)
                {
                    group.GroupingStatus = "Finalized";
                }

                // --- Sanity Check Logic ---
                var potentiallyStuckJobs = _jobGroups.Values
                    .SelectMany(g => g.Jobs)
                    .Where(j => (j.Status.Contains("Printing") || j.Status.Contains("Spooling")) && (DateTime.Now - j.SubmittedAt).TotalSeconds > 5)
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
            if (needsUiRefresh || groupsToFinalize.Any())
            {
                _view.DisplayJobGroups(_jobGroups.Values.ToList());
            }
        }

        public void Start()
        {
            _printJobService.StartMonitoring();
            _uiRefreshTimer.Start();
        }
    }
}