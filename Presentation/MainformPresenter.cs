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
        private readonly List<PrintJob> _jobs;

        public MainFormPresenter(IMainFormView view, IPrintJobService printJobService)
        {
            _view = view;
            _printJobService = printJobService;
            _jobs = new List<PrintJob>();
         
            _printJobService.JobSpooling += OnJobSpooling;
        }

        private void OnJobSpooling(PrintJob newJob)
        {
            
            _jobs.Add(newJob);

        
            _view.DisplayJobs(_jobs.ToList());
        }

        public void Start()
        {
            _printJobService.StartMonitoring();
        }
    }
}
