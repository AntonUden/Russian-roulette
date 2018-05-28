using System;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Russian_roulette
{
    class Program
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void Main(string[] args)
        {
            if(IsAdministrator())
            {
                Random r = new Random();
                if (r.Next(6) == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bang");
                    try
                    {
                        int isCritical = 1;
                        int BreakOnTermination = 0x1D;
                        Process.EnterDebugMode();
                        NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
                        Thread.Sleep(1000);
                        Process.GetCurrentProcess().Kill();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error, could not set critical process");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Click");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Run as admin to play");
            }
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
