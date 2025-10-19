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

        private readonly Dictionary<string, PrintJobGroup> _jobGroups = new();
        private readonly object _lock = new();
        private readonly System.Windows.Forms.Timer _uiRefreshTimer;

        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;

          
            _printJobService.JobSpooling += OnJobReceived;
            _printJobService.JobUpdated += OnJobReceived;
            _printJobService.JobDeleted += OnJobDeleted;

         
            _uiRefreshTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiRefreshTimer.Tick += UiRefreshTimer_Tick;
        }

       
        private void OnJobReceived(PrintJob job)
        {
            lock (_lock)
            {
                string baseDocName = GetBaseDocumentName(job.DocumentName);
                string groupKey = $"{job.User}|{baseDocName}".ToLower();

                if (!_jobGroups.TryGetValue(groupKey, out var group))
                {
                   
                    group = new PrintJobGroup(groupKey);
                    _jobGroups.Add(groupKey, group);
                }

                var existingJob = group.Jobs.FirstOrDefault(j => j.JobId == job.JobId);

                if (existingJob != null)
                {
                 
                    group.Jobs.Remove(existingJob);
                    group.Jobs.Add(job);
                }
                else
                {
                    
                    group.Jobs.Add(job);
                    group.LastActivity = DateTime.Now;        
                    group.GroupingStatus = "Grouping";        
                }
            }

      
            _view.DisplayJobGroups(_jobGroups.Values.ToList());
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
                        jobInGroup.Status = "Completed";
                }
            }

            _view.DisplayJobGroups(_jobGroups.Values.ToList());
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
                _view.DisplayJobGroups(_jobGroups.Values.ToList());
        }

       
        public void Start()
        {
            _printJobService.StartMonitoring();
            _uiRefreshTimer.Start();
        }
    }
}
