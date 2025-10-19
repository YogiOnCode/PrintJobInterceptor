using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PrintJobInterceptor.Core.Services
{
    public enum TestScenario
    {
      
        InteractiveTest,
        GroupedJobWithTimeout,
        SequentialJobNames,

        SingleJobs,
        PausingJob,
        CancellingJob,
        ErrorJob
    }

    public class MockPrintJobService : IPrintJobService, IDisposable
    {
        public event Action<PrintJob> JobSpooling;
        public event Action<PrintJob> JobUpdated;
        public event Action<PrintJob> JobDeleted;

        private readonly ConcurrentDictionary<int, PrintJob> _activeSimulatedJobs = new ConcurrentDictionary<int, PrintJob>();

        private const int GROUPING_TIMEOUT_MS = 5000;
        private readonly Random _random = new Random();
        private CancellationTokenSource _cancellationTokenSource;

        public void PauseJob(int jobId, string printerName)
        {
            if (_activeSimulatedJobs.TryGetValue(jobId, out var job))
            {
                job.Status = "Paused";
                JobUpdated?.Invoke(job);
            }
        }

        public void ResumeJob(int jobId, string printerName)
        {
            if (_activeSimulatedJobs.TryGetValue(jobId, out var job))
            {
                job.Status = "Printing";
                JobUpdated?.Invoke(job);
            }
        }

        public void CancelJob(int jobId, string printerName)
        {
            if (_activeSimulatedJobs.TryGetValue(jobId, out var job))
            {
                job.Status = "Deleting";
                JobUpdated?.Invoke(job);
            }
        }

        public void StartMonitoring() // Change the TestScenerario here to test different scenarios
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunTest(TestScenario.InteractiveTest, _cancellationTokenSource.Token);
          
        }

        public void StopMonitoring()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            Console.WriteLine("(Mock) Monitoring stopped.");
        }

        public async void RunTest(TestScenario scenario, CancellationToken token)
        {
            try
            {
                await Task.Delay(1000, token);

                switch (scenario)
                {
                   
                    case TestScenario.InteractiveTest:
                        await SimulateInteractiveTest(5, token);
                        break;
                    case TestScenario.GroupedJobWithTimeout:
                        await SimulateGroupedJobWithTimeout(3, token);
                        break;
                    case TestScenario.SequentialJobNames:
                        await SimulateSequentialJobNames(token);
                        break;
                    case TestScenario.PausingJob:
                        await SimulatePausingJob("Financials.xlsx", "Bob", token);
                        break;
                    case TestScenario.CancellingJob:
                        await SimulateCancellingJob("Temp-Document.docx", "Charlie", token);
                        break;
                    case TestScenario.SingleJobs:
                    default:
                        await SimulateSingleJobs(2, token);
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("(Mock) Test scenario was cancelled.");
            }
        }

       
        private async Task SimulateInteractiveTest(int jobCount, CancellationToken token)
        {
            Console.WriteLine(" Interactive Test Scenario" );
            for (int i = 0; i < jobCount; i++)
            {
                var pageCount = 50 + _random.Next(1, 150); 
                var job = CreateJob($"Large-Project-File-{i + 1}.dwg", "Yogiy", pageCount);

               
                SimulateJobLifecycle(job, token, lifecycleDurationMs: 30000);

                await Task.Delay(4000, token); 
            }
        }

      
        private async Task SimulateGroupedJobWithTimeout(int jobCount, CancellationToken token)
        {
            Console.WriteLine(" Running Grouped Job w/ Timeout Scenario");
            string docName = $"Quarterly-Report-{_random.Next(100, 999)}.pdf";

          
            for (int i = 1; i <= jobCount; i++)
            {
                var job = CreateJob(docName, "Alice");
                SimulateJobLifecycle(job, token);
                await Task.Delay(1500, token);
            }

          
            Console.WriteLine($"(Mock) Waiting for {GROUPING_TIMEOUT_MS}ms timeout...");
            await Task.Delay(GROUPING_TIMEOUT_MS + 2000, token);

        
            Console.WriteLine("(Mock) Sending one more job after timeout.");
            var lateJob = CreateJob(docName, "Alice");
            SimulateJobLifecycle(lateJob, token);
        }

        private async Task SimulateSequentialJobNames(CancellationToken token)
        {
            Console.WriteLine(" Running Sequential Job Names Scenario ");
            string baseName = "MyReport";

         
            var job1 = CreateJob($"{baseName}_001.pdf", "Bob");
            SimulateJobLifecycle(job1, token);
            await Task.Delay(1500, token);

          
            var job2 = CreateJob($"{baseName}_002.pdf", "Bob");
            SimulateJobLifecycle(job2, token, pauseDurationMs: 4000);
            await Task.Delay(1500, token);

          
            var job3 = CreateJob($"{baseName} (Part 3).pdf", "Bob");
            SimulateJobLifecycle(job3, token, cancelJob: true);
        }

     

        private async Task SimulatePausingJob(string docName, string user, CancellationToken token)
        {
            var job = CreateJob(docName, user);
            await SimulateJobLifecycle(job, token, pauseDurationMs: 5000);
        }

        private async Task SimulateCancellingJob(string docName, string user, CancellationToken token)
        {
            var job = CreateJob(docName, user);
            await SimulateJobLifecycle(job, token, cancelJob: true);
        }

        private async Task SimulateSingleJobs(int count, CancellationToken token)
        {
            for (int i = 0; i < count; i++)
            {
                var job = CreateJob($"Single-Doc-{i}.pdf", "Charlie");
                SimulateJobLifecycle(job, token);
                await Task.Delay(2000, token);
            }
        }

        private async Task SimulateJobLifecycle(PrintJob job, CancellationToken token, int lifecycleDurationMs = 15000, int pauseDurationMs = 0, bool cancelJob = false)
        {
            _activeSimulatedJobs.TryAdd(job.JobId, job);

            try
            {
               
                job.Status = "Spooling";
                JobSpooling?.Invoke(job);
                await Task.Delay(2000, token); 

               
                if (job.Status == "Spooling") 
                {
                    job.Status = "Printing";
                    JobUpdated?.Invoke(job);
                }

                int progress = 0;
                int totalDuration = 30000; 

              
                while (progress < totalDuration && !token.IsCancellationRequested)
                {
                    if (!_activeSimulatedJobs.TryGetValue(job.JobId, out var currentJobState)) break;

                    switch (currentJobState.Status)
                    {
                        case "Paused":
                           
                            await Task.Delay(500, token);
                            break;

                        case "Printing":
                          
                            await Task.Delay(1000, token);
                            progress += 1000;
                            break;

                        case "Deleting":
                        case "Error":
                          
                            progress = totalDuration;
                            break;
                    }
                }

           
                if (_activeSimulatedJobs.TryGetValue(job.JobId, out var finalJobState))
                {
                    if (finalJobState.Status == "Printing")
                    {
                        
                        finalJobState.Status = "Completed";
                    }
                   
                    JobDeleted?.Invoke(finalJobState);
                }
            }
            catch (TaskCanceledException) {  }
            finally
            {
               
                _activeSimulatedJobs.TryRemove(job.JobId, out _);
            }
        }

        private PrintJob CreateJob(string docName, string user, int? pageCount = null)
        {
            return new PrintJob
            {
                JobId = _random.Next(1000, 9999),
                DocumentName = docName,
                User = user,
                Status = "Unknown",
                PageCount = pageCount ?? _random.Next(1, 20),
                PrinterName = "Mock Printer",
                SubmittedAt = DateTime.Now
            };
        }
        public bool DoesJobExist(int jobId, string printerName)
        {
           
            return _activeSimulatedJobs.ContainsKey(jobId);
        }
        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}