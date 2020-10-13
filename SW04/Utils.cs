using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Explorer700Wrapper
{
    class Utils
    {
        /// <summary>
        /// Waits for debugger to be attached before proceeding.
        /// </summary>
        /// <returns>True if the debugger is attached, false otherwise.</returns>
        public static bool WaitForDebugger()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (!args.Any(arg => arg == "--debug"))
                return false;

            Console.WriteLine("Waiting for debugger. Press Enter to continue.");
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                    return false;

                if (Debugger.IsAttached)
                    return true;

                Thread.Sleep(100);
            }
        }
    }
}
