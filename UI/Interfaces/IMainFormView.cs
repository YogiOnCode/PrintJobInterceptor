using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintJobInterceptor.UI.Interfaces
{
    public interface IMainFormView
    {
        void DisplayJobGroups(IEnumerable<PrintJobGroup> groups);
        void ShowNotification(string message, FeedbackType type);

        event Action<int> PauseJobRequested;
        event Action<int> ResumeJobRequested;
        event Action<int> CancelJobRequested;
    }
}
