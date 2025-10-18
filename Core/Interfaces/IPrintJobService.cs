using PrintJobInterceptor.Core.Models;
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

        void StartMonitoring();
        void StopMonitoring();
        void PauseJob(int jobId);
        void ResumeJob(int jobId);
        void CancelJob(int jobId);
    }
}
