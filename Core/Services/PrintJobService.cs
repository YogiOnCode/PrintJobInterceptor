using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Collections.Generic;
using System.Management;

namespace PrintJobInterceptor.Core.Services
{
    public class PrintJobService : IPrintJobService, IDisposable
    {
        private ManagementEventWatcher _creationWatcher;
        private ManagementEventWatcher _modificationWatcher;
        private ManagementEventWatcher _deletionWatcher;

        private readonly Dictionary<int, PrintJob> _activeJobs = new Dictionary<int, PrintJob>();
        private readonly object _lock = new object();


        public event Action<PrintJob> JobSpooling;
        public event Action<PrintJob> JobUpdated;
        public event Action<PrintJob> JobDeleted;

        public PrintJobService()
        {

            var creationQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PrintJob'");
            _creationWatcher = new ManagementEventWatcher(creationQuery);
            _creationWatcher.EventArrived += OnJobCreation;

            var modificationQuery = new WqlEventQuery("SELECT * FROM __InstanceModificationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PrintJob'");
            _modificationWatcher = new ManagementEventWatcher(modificationQuery);
            _modificationWatcher.EventArrived += OnJobModification;

            var deletionQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PrintJob'");
            _deletionWatcher = new ManagementEventWatcher(deletionQuery);
            _deletionWatcher.EventArrived += (s, e) => HandleEvent(e, JobDeleted);

        }

        private void HandleEvent(EventArrivedEventArgs e, Action<PrintJob> eventToRaise)
        {
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            if (targetInstance == null) return;

            var printJob = CreatePrintJobFromWmiObject(targetInstance);
            eventToRaise?.Invoke(printJob);
        }

        private void OnJobCreation(object sender, EventArrivedEventArgs e)
        {
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            if (targetInstance == null) return;

            var printJob = CreatePrintJobFromWmiObject(targetInstance);

            lock (_lock)
            {
                if (!_activeJobs.ContainsKey(printJob.JobId))
                {
                    _activeJobs.Add(printJob.JobId, printJob);
                }
            }
            JobSpooling?.Invoke(printJob);
        }
        private void OnJobModification(object sender, EventArrivedEventArgs e)
        {
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            if (targetInstance == null) return;

            var updatedJob = CreatePrintJobFromWmiObject(targetInstance);

            lock (_lock)
            {
                if (_activeJobs.ContainsKey(updatedJob.JobId))
                {
                  
                    _activeJobs[updatedJob.JobId] = updatedJob;
                }
            }
            JobUpdated?.Invoke(updatedJob);
        }
        private PrintJob CreatePrintJobFromWmiObject(ManagementBaseObject wmiObject)
        {
            return new PrintJob
            {
                JobId = Convert.ToInt32(wmiObject["JobId"]),
                DocumentName = wmiObject["Document"].ToString(),
                PrinterName = wmiObject["Name"].ToString().Split(',')[0],
                User = wmiObject["Owner"].ToString(),
                Status = wmiObject["JobStatus"]?.ToString() ?? "Unknown",
                PageCount = Convert.ToInt32(wmiObject["TotalPages"]),
                SizeInBytes = Convert.ToUInt32(wmiObject["Size"]), 
                SubmittedAt = ManagementDateTimeConverter.ToDateTime(wmiObject["TimeSubmitted"].ToString())
            };
        }

        public void StartMonitoring()
        {
            _creationWatcher.Start();
            _modificationWatcher.Start();
            _deletionWatcher.Start();
        }

        public void StopMonitoring()
        {
            _creationWatcher.Stop();
            _modificationWatcher.Stop();
            _deletionWatcher.Stop();
        }


        public void PauseJob(int jobId) => throw new NotImplementedException();
        public void ResumeJob(int jobId) => throw new NotImplementedException();
        public void CancelJob(int jobId) => throw new NotImplementedException();
        public void SimulateGroupedJob() { }
        
      
        public void Dispose()
        {
            StopMonitoring();
            _creationWatcher.Dispose();
            _modificationWatcher.Dispose();
        }
    }
}
