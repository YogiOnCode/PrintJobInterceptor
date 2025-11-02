using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintJobInterceptor.Core.Models
{

    public class PrintJobGroup
    {
        [JsonIgnore]
        public string GroupKey { get; }


        public string DocumentName => Jobs.FirstOrDefault()?.DocumentName ?? "Unknown";
        public string User => Jobs.FirstOrDefault()?.User ?? "Unknown";
        public string PrinterName => Jobs.FirstOrDefault()?.PrinterName ?? "Unknown";
        public string DocumentType => Jobs.FirstOrDefault()?.DocumentType ?? "Unknown";
        public int TotalPages => Jobs.Sum(j => j.PageCount);


        [JsonIgnore]
        public long TotalSizeInBytes => Jobs.Sum(j => j.SizeInBytes);

        [JsonIgnore]
        public string GroupingStatus { get; set; }

        public int JobCount => Jobs.Count;
        public string Status
        {
            get
            {

                if (Jobs.Any(j => j.Status.Contains("Error")))
                    return "Error";


                if (Jobs.Any(j => j.Status.Contains("Delet") || j.Status.Contains("Cancel")))
                    return "Deleting/Cancelled";


                if (Jobs.Any(j => j.Status.Contains("Paused")))
                    return "Paused";

                if (Jobs.All(j => j.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)))
                    return "Finished";


                var firstActiveJob = Jobs.FirstOrDefault(j => !j.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase));
                if (firstActiveJob != null)
                {
                    return firstActiveJob.Status;
                }


                return "Unknown";
            }
        }

        public List<PrintJob> Jobs { get; } = new List<PrintJob>();

        public DateTime LastActivity { get; set; }

        public PrintJobGroup(string key)
        {
            GroupKey = key;
            LastActivity = DateTime.Now;
        }

    }
}
