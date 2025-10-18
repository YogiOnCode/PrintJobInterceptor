using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Threading.Tasks;

namespace PrintJobInterceptor.Core.Services
{
    public enum TestScenario
    {
        SingleJobs,
        GroupedJobWithTimeout,
        EdgeCases,
        SequentialJobNames
    }

    public enum TestUser
    {
        Alice,
        Bob,
        Charlie,
        Yogiy
    }

    public class MockPrintJobService : IPrintJobService
    {
        public event Action<PrintJob> JobSpooling;
        public event Action<PrintJob> JobUpdated;
        public event Action<PrintJob> JobDeleted;
        private const int GROUPING_TIMEOUT_MS = 5000;
        private readonly Random _random = new Random();

        public void StopMonitoring() { }
        public void PauseJob(int jobId) { }
        public void ResumeJob(int jobId) { }
        public void CancelJob(int jobId) { }

        public void StartMonitoring()
        {
           
            RunTest(TestScenario.SequentialJobNames);
        }
        public async void RunTest(TestScenario scenario)
        {
            
            await Task.Delay(1000);

            switch (scenario)
            {
                case TestScenario.GroupedJobWithTimeout:
                    await SimulateGroupedJobWithTimeout(5);
                    break;
                case TestScenario.EdgeCases:
                    await SimulateEdgeCases();
                    break;
                case TestScenario.SequentialJobNames:
                    await SimulateSequentialJobNames();
                    break;
                case TestScenario.SingleJobs:
                default:
                    await SimulateSingleJobs(3);
                    break;
            }
        }
        private async Task SimulateGroupedJobWithTimeout(int jobCount)
        {
            string docName = $"Grouped-Report-{_random.Next(100, 999)}.pdf";
            for (int i = 1; i <= jobCount; i++)
            {
                FireJobEvent(docName);
                await Task.Delay(500);
            }
            await Task.Delay(GROUPING_TIMEOUT_MS + 1000);
            FireJobEvent(docName);
        }
        private async Task SimulateSequentialJobNames()
        {
            string baseName = "MyReport";
            FireJobEvent($"{baseName}_001.pdf");
            await Task.Delay(500);
            FireJobEvent($"{baseName}_002.pdf");
            await Task.Delay(500);
            FireJobEvent($"{baseName} (Part 3).pdf"); // A different pattern
        }
        private async Task SimulateEdgeCases()
        {
           
            FireJobEvent("Annual-Report.pdf", "UserA");
            await Task.Delay(100);
            FireJobEvent("Marketing-Flyer.docx", "UserB");

            await Task.Delay(GROUPING_TIMEOUT_MS + 3000);

            FireJobEvent("Shared-Report.pdf", "Alice");
            await Task.Delay(1000);
            FireJobEvent("Shared-Report.pdf", "Bob");
        }

        private async Task SimulateSingleJobs(int jobCount)
        {
            for (int i = 0; i < jobCount; i++)
            {
                FireJobEvent($"Single-Doc-{i}.pdf");
                await Task.Delay(1000);
            }
        }
        private void FireJobEvent(string docName, string user)
        {
            var job = CreateJob(docName, user);
            JobSpooling?.Invoke(job);
        }
        private void FireJobEvent(string docName)
        {
            var user = GetRandomUser().ToString();
            var job = CreateJob(docName, user);
            JobSpooling?.Invoke(job);
        }

        private PrintJob CreateJob(string docName, string user)
        {
            return new PrintJob
            {
                JobId = _random.Next(1000, 9999),
                DocumentName = docName,
                User = user,
                Status = "Printing",
                PageCount = _random.Next(1, 20),
                PrinterName = "Mock Printer",
                SubmittedAt = DateTime.Now
            };
        }
        private TestUser GetRandomUser()
        {
            var users = Enum.GetValues(typeof(TestUser));
            return (TestUser)users.GetValue(_random.Next(users.Length));
        }
       
        public void SimulateGroupedJob() { }


    }
}