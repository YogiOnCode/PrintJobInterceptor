using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.UI.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PrintJobInterceptor.Presentation
{
    public class MainFormPresenter
    {
        private readonly IMainFormView _view;
        private readonly IPrintJobService _printJobService;

        private readonly Dictionary<int, PrintJob> _jobs;
        private readonly object _lock = new object();

        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;
            _jobs = new Dictionary<int, PrintJob>();

            _printJobService.JobSpooling += OnJobSpooling;
            _printJobService.JobUpdated += OnJobUpdated;
        }

        private void OnJobSpooling(PrintJob newJob)
        {

            UpdateAndRefresh(newJob);
         
        }
        private void UpdateAndRefresh(PrintJob job)
        {
            lock (_lock)
            {
               
                _jobs[job.JobId] = job;
            }

          
            _view.DisplayJobs(_jobs.Values.ToList());
        }
        private void OnJobUpdated(PrintJob updatedJob)
        {
            UpdateAndRefresh(updatedJob);
        }
        public void Start()
        {
            _printJobService.StartMonitoring();
        }
    }
}
