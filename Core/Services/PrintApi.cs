using System;
using System.Runtime.InteropServices;

namespace PrintJobInterceptor.Core.Services
{
    public static class PrintApi
    {
        public enum JobCommand
        {
            Pause = 1,
            Resume = 2,
            Cancel = 3,
            Restart = 4
        }
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClosePrinter(IntPtr hPrinter);


        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetJob(IntPtr hPrinter, int JobId, int Level, IntPtr pJob, int Command);


    }
}
