using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<PrintJobService> _logger;

        private readonly Dictionary<int, PrintJob> _activeJobs = new Dictionary<int, PrintJob>();
        private readonly object _lock = new object();


        public event Action<PrintJob> JobSpooling;
        public event Action<PrintJob> JobUpdated;
        public event Action<PrintJob> JobDeleted;

        public PrintJobService(ILogger<PrintJobService> logger)
        {
            _logger = logger;
            _logger.LogInformation("PrintJobService initializing.");

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


        private void OnJobCreation(object sender, EventArrivedEventArgs e)
        {
            var targetInstance = e.NewEvent["TargetInstance"] as ManagementBaseObject;
            if (targetInstance == null)
            {
                _logger.LogWarning("OnJobCreation event fired but TargetInstance was null.");
                return;
            }

            var printJob = CreatePrintJobFromWmiObject(targetInstance);
            _logger.LogInformation(
                "Job Creation: JobID {JobID} for {User} on {Printer}. Doc: '{Document}', Status: {Status}",
                printJob.JobId, printJob.User, printJob.PrinterName, printJob.DocumentName, printJob.Status);


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
            if (targetInstance == null)
            {
                _logger.LogWarning("OnJobModification event fired but TargetInstance was null.");
                return;
            }

            var updatedJob = CreatePrintJobFromWmiObject(targetInstance);
            _logger.LogDebug(
                "Job Modification: JobID {JobID}, Status: {Status}, Pages: {Pages}",
                updatedJob.JobId, updatedJob.Status, updatedJob.PageCount);

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
            if (targetInstance == null)
            {
                _logger.LogWarning("OnJobDeletion event fired but TargetInstance was null.");
                return;
            }

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
                _logger.LogInformation(
                    "Job Deletion: JobID {JobID}, Final Status: '{Status}'",
                    finalJobState.JobId, finalJobState.Status);
                JobDeleted?.Invoke(finalJobState);
            }
            else
            {
                var printJob = CreatePrintJobFromWmiObject(targetInstance);
                _logger.LogInformation(
                    "Job Deletion (from WMI): JobID {JobID}, Status: '{Status}'",
                    printJob.JobId, printJob.Status);
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
        public bool DoesJobExist(int jobId, string printerName)
        {
            _logger.LogDebug("Checking existence of JobID {JobID} on {PrinterName}.", jobId, printerName);
            try
            {
               
                string wmiQuery = $"SELECT JobId FROM Win32_PrintJob WHERE JobId = {jobId} AND Name LIKE '{printerName.Replace("\\", "\\\\")}%'";

                using (var searcher = new ManagementObjectSearcher(wmiQuery))
                using (var results = searcher.Get())
                {
                    bool exists = results.Count > 0;
                    _logger.LogDebug("Job existence check for JobID {JobID} returned {Exists}.", jobId, exists);
                    return exists;
                }
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "WMI check for JobID {JobID} failed. Returning false.", jobId);
                return false; 
            }
        }
        public void StartMonitoring()
        {
            _logger.LogInformation("Starting WMI print job monitoring.");
            _creationWatcher.Start();
            _modificationWatcher.Start();
            _deletionWatcher.Start();
        }

        public void StopMonitoring()
        {
            _logger.LogInformation("Stopping WMI print job monitoring.");
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
                    _logger.LogError(
                         "Failed to open printer {PrinterName} to {Command} JobID {JobID}. Win32Error: {Error}",
                         printerName, command, jobId, Marshal.GetLastWin32Error());
                    return false;
                }

               
                bool success = PrintApi.SetJob(hPrinter, jobId, 0, IntPtr.Zero, (int)command);
                if (!success)
                {
                    _logger.LogWarning(
                        "Failed to execute {Command} on JobID {JobID} on {PrinterName}. Win32Error: {Error}",
                        command, jobId, printerName, Marshal.GetLastWin32Error());
                }
                else
                {
                    _logger.LogInformation(
                         "Successfully executed {Command} on JobID {JobID} on {PrinterName}.",
                         command, jobId, printerName);
                }
                return success;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unhandled exception in ControlJob while trying to {Command} JobID {JobID}.", command, jobId);
                return false;
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
