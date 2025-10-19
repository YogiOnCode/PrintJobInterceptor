using System;
using System.Windows.Forms;

namespace PrintJobInterceptor.UI.Helpers
{
    public static class UIExtensions
    {
       
        public static void SafeInvoke(this Control control, Action action)
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
