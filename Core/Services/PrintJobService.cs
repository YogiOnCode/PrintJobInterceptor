using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Documents;

namespace PrintJobInterceptor.Core.Services
{
    public enum JobAction
    {
        Pause,
        Resume,
        Cancel
    }

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
            _deletionWatcher.EventArrived += OnJobDeletion;

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
            Debug.WriteLine($"[Job Creation] JobId: {printJob.JobId}, Document: '{printJob.DocumentName}', Raw Status: '{printJob.Status}', DataType: '{printJob.DocumentType}'");
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
            Debug.WriteLine($"[Job Modification] JobId: {updatedJob.JobId}, Raw Status: '{updatedJob.Status}'");

            lock (_lock)
            {
                if (_activeJobs.ContainsKey(updatedJob.JobId))
                {
                  
                    _activeJobs[updatedJob.JobId] = updatedJob;
                }
            }
            JobUpdated?.Invoke(updatedJob);
        }
        private void OnJobDeletion(object sender, EventArrivedEventArgs e)
        {
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            if (targetInstance == null) return;

            var jobId = Convert.ToInt32(targetInstance["JobId"]);

            PrintJob finalJobState = null;
            lock (_lock)
            {
                if (_activeJobs.TryGetValue(jobId, out finalJobState))
                {
                    
                    _activeJobs.Remove(jobId);
                }
            }

            if (finalJobState != null)
            {
                Debug.WriteLine($"[Job Deletion] JobId: {finalJobState.JobId}, Final Cached Status: '{finalJobState.Status}'");
                JobDeleted?.Invoke(finalJobState);
            }
            else
            {
                var printJob = CreatePrintJobFromWmiObject(targetInstance);
                JobDeleted?.Invoke(printJob);
            }
        }
        private string DetermineDocumentType(string wmiDataType, string documentName)
        {
            string docNameLower = documentName.ToLower();

            
            if (docNameLower.Contains(".txt"))
                return "Text Document";
            if (docNameLower.Contains(".pdf"))
                return "PDF Document";
            if (docNameLower.Contains(".jpg") || docNameLower.Contains(".jpeg") || docNameLower.Contains(".png") || docNameLower.Contains(".bmp") || docNameLower.Contains("photo"))
                return "Image";
            if (docNameLower.Contains(".docx") || docNameLower.Contains(".doc"))
                return "Word Document";
            if (docNameLower.Contains(".xlsx") || docNameLower.Contains(".xls"))
                return "Excel Document";

           
            if (!string.IsNullOrEmpty(wmiDataType))
            {
                if (wmiDataType.Equals("TEXT", StringComparison.OrdinalIgnoreCase))
                {
                    return "Text"; 
                }
                if (wmiDataType.Contains("EMF"))
                {
                    return "Mixed Content";
                }
            }

           
            return !string.IsNullOrEmpty(wmiDataType) ? wmiDataType : "Unknown";
        }
        private PrintJob CreatePrintJobFromWmiObject(ManagementBaseObject wmiObject)
        {
            string docName = wmiObject["Document"].ToString();
            string dataType = wmiObject["DataType"]?.ToString();

            return new PrintJob
            {
                JobId = Convert.ToInt32(wmiObject["JobId"]),
                DocumentName = docName,
                DocumentType = DetermineDocumentType(dataType, docName),
                PrinterName = wmiObject["Name"].ToString().Split(',')[0],
                User = wmiObject["Owner"].ToString(),
                Status = wmiObject["JobStatus"]?.ToString() ?? "Unknown",
                PageCount = Convert.ToInt32(wmiObject["TotalPages"]),
                SizeInBytes = Convert.ToUInt32(wmiObject["Size"]),
                SubmittedAt = ManagementDateTimeConverter.ToDateTime(wmiObject["TimeSubmitted"].ToString()),
              
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


        public void PauseJob(int jobId, string printerName)
        {
            ControlJob(printerName, jobId, PrintApi.JobCommand.Pause);
        }

        public void ResumeJob(int jobId, string printerName)
        {
            ControlJob(printerName, jobId, PrintApi.JobCommand.Resume);
        }

        public void CancelJob(int jobId, string printerName)
        {
            ControlJob(printerName, jobId, PrintApi.JobCommand.Cancel);
        }
        private bool ControlJob(string printerName, int jobId, PrintApi.JobCommand command)
        {
            IntPtr hPrinter = IntPtr.Zero;
            try
            {
               
                if (!PrintApi.OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
                {
                    Debug.WriteLine($"Failed to open printer: {printerName}. Error code: {Marshal.GetLastWin32Error()}");
                    return false;
                }

               
                bool success = PrintApi.SetJob(hPrinter, jobId, 0, IntPtr.Zero, (int)command);
                if (!success)
                {
                    Debug.WriteLine($"Failed to execute command '{command}' on JobId {jobId}. Error code: {Marshal.GetLastWin32Error()}");
                }
                else
                {
                    Debug.WriteLine($"Successfully executed command '{command}' on JobId {jobId}.");
                }
                return success;
            }
            finally
            {
                if (hPrinter != IntPtr.Zero)
                {
                    PrintApi.ClosePrinter(hPrinter);
                }
            }
        }





        public void SimulateGroupedJob() { }
        
      
        public void Dispose()
        {
            StopMonitoring();
            _creationWatcher.Dispose();
            _modificationWatcher.Dispose();
        }
    }
}
