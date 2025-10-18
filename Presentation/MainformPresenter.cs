using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.UI.Interfaces;
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

        private readonly Dictionary<string, PrintJobGroup> _activeGroups = new Dictionary<string, PrintJobGroup>();
        private readonly List<PrintJobGroup> _finalizedGroups = new List<PrintJobGroup>();
        private readonly object _lock = new object();
        private readonly System.Windows.Forms.Timer _uiRefreshTimer;

        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;

        
            _printJobService.JobSpooling += OnJobReceived;
            _printJobService.JobUpdated += OnJobReceived;

            _uiRefreshTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiRefreshTimer.Tick += UiRefreshTimer_Tick;
        }

        private void OnJobReceived(PrintJob job)
        {
            lock (_lock)
            {
                string groupKey = $"{job.User}|{job.DocumentName}".ToLower();

                if (_activeGroups.TryGetValue(groupKey, out var group))
                {
                 
                    var existingJob = group.Jobs.FirstOrDefault(j => j.JobId == job.JobId);
                    if (existingJob != null) { group.Jobs.Remove(existingJob); }
                    group.Jobs.Add(job);
                    group.LastActivity = DateTime.Now;
                }
                else
                {
                   
                    var newGroup = new PrintJobGroup(groupKey);
                    newGroup.Jobs.Add(job);
                    _activeGroups.Add(groupKey, newGroup);
                }
            }
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            lock (_lock)
            {
              
                var groupsToFinalize = _activeGroups.Values
                    .Where(g => (DateTime.Now - g.LastActivity).TotalMilliseconds > GROUPING_TIMEOUT_MS)
                    .ToList();

                foreach (var group in groupsToFinalize)
                {
                    group.GroupingStatus = "Finalized";
                    _activeGroups.Remove(group.GroupKey);
                    _finalizedGroups.Add(group);
                }
            }

           
            var allGroups = _finalizedGroups.Concat(_activeGroups.Values).ToList();
            _view.DisplayJobGroups(allGroups);
        }

        public void Start()
        {
            _printJobService.StartMonitoring();
            _uiRefreshTimer.Start();
        }
    }
}