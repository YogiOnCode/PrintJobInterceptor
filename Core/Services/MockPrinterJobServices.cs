using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Threading.Tasks;

namespace PrintJobInterceptor.Core.Services
{
    public class MockPrintJobService : IPrintJobService
    {
        public event Action<PrintJob> JobSpooling;
        public event Action<PrintJob> JobUpdated;

        private const int GROUPING_TIMEOUT_MS = 5000;

        public void StopMonitoring() { }
        public void PauseJob(int jobId) { }
        public void ResumeJob(int jobId) { }
        public void CancelJob(int jobId) { }

        public void StartMonitoring()
        {
            SimulateGroupedJob();
        }

        public async void SimulateGroupedJob()
        {
            var random = new Random();
            string docName = $"Simulated-Report-{random.Next(100, 999)}.pdf";
            string user = "TestUser";


            for (int i = 1; i <= 5; i++)
            {
                var job = new PrintJob
                {
                    JobId = random.Next(1000, 9999),
                    DocumentName = docName,
                    User = user,
                    Status = "Printing",
                    PageCount = 10,
                    SubmittedAt = DateTime.Now
                };
                JobSpooling?.Invoke(job);
                await Task.Delay(500); // 0.5 second delay between jobs
            }

            await Task.Delay(GROUPING_TIMEOUT_MS + 1000);
            var finalJob = new PrintJob
            {
                JobId = random.Next(1000, 9999),
                DocumentName = docName, // Same name and user
                User = user,
                Status = "Printing",
                PageCount = 2,
                SubmittedAt = DateTime.Now
            };
            JobSpooling?.Invoke(finalJob);
        }
    }
}