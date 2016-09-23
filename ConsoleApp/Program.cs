using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProcessHasWindow
{
    class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"\r\nReports whether a process has one or multiple visible (covered or uncovered) windows.\r\n\r\nUsage: {AppDomain.CurrentDomain.FriendlyName} processname");
                return 2;
            }
            var processName = new Regex(@"\.exe\s*$").Replace(args[0], "");
            Process[] processes = Process.GetProcessesByName(processName);
            Thread.Sleep(100);
            Console.WriteLine($"Found {processes.Length} {processName} processes.");
            foreach (Process p in processes)
            {
                IntPtr windowHandle = p.MainWindowHandle;
                if ((int) windowHandle != 0)
                {
                    var isWindow = IsWindow(windowHandle);
                    if (isWindow)
                    {
                        Console.WriteLine($"{processName} has at least one visible window.");
                        return 1;
                    }
                }
            }
            Console.WriteLine($"{processName} does not have a visible window.");
            return 0;
        }
    }
}
