using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Management;

namespace PrintJobInterceptor.Core.Services
{
    public class PrintJobService : IPrintJobService, IDisposable
    {
        private ManagementEventWatcher _watcher;

        public event Action<PrintJob> JobSpooling;

        public PrintJobService()
        {
          
            var query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PrintJob'");
            _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += Watcher_EventArrived;
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
           
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;

            if (targetInstance != null)
            {
                // Create our custom PrintJob object from the WMI data.
                var printJob = new PrintJob
                {
                    JobId = Convert.ToInt32(targetInstance["JobId"]),
                    DocumentName = targetInstance["Document"].ToString(),
                    PrinterName = targetInstance["Name"].ToString().Split(',')[0],
                    User = targetInstance["Owner"].ToString(),
                    Status = targetInstance["JobStatus"]?.ToString() ?? "Unknown",
                    PageCount = Convert.ToInt32(targetInstance["TotalPages"]),
                    SizeInBytes = Convert.ToInt64(targetInstance["Size"]),
                    SubmittedAt = ManagementDateTimeConverter.ToDateTime(targetInstance["TimeSubmitted"].ToString())
                };

               
                JobSpooling?.Invoke(printJob);
            }
        }

        public void StartMonitoring()
        {
            _watcher.Start();
        }

        public void StopMonitoring()
        {
            _watcher.Stop();
        }

    
        public void PauseJob(int jobId) => throw new NotImplementedException();
        public void ResumeJob(int jobId) => throw new NotImplementedException();
        public void CancelJob(int jobId) => throw new NotImplementedException();

        public void Dispose()
        {
            StopMonitoring();
            _watcher.Dispose();
        }
    }
}
