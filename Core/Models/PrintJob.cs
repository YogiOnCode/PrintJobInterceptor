using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintJobInterceptor.Core.Models
{
    public class PrintJob
    {
        public int JobId { get; set; }
        public string DocumentName { get; set; }
        public string PrinterName { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int PageCount { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string DocumentType { get; set; }
        public long SizeInBytes { get; set; }
    }
}
