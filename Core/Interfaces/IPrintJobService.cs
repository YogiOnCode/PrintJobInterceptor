using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.Core.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintJobInterceptor.Core.Interfaces
{
    public interface IPrintJobService
    {
        event Action<PrintJob> JobSpooling;
        event Action<PrintJob> JobUpdated;
        event Action<PrintJob> JobDeleted;

        void StartMonitoring();
        void StopMonitoring();
        void PauseJob(int jobId, string printerName);
        void ResumeJob(int jobId, string printerName);
        void CancelJob(int jobId, string printerName);
        bool DoesJobExist(int jobId, string printerName);
        void RunTest(TestScenario scenario);
    }
}
