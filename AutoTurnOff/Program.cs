using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTurnOff
{
    static class Program
    {
        private const string appName = "AutoTurnOff";
        private static string path = AppDomain.CurrentDomain.BaseDirectory + appName + ".exe";
        private static RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private static int secontsTillShutDown = 30;
        private static int ShutDownAt = 2;

        static void Main(string[] args)
        {
            Console.WriteLine("AutoTurnOff is running!");

            Task.Factory.StartNew(SkipTimer);

            if (!IsStartupItem())
            {
                Console.WriteLine($"AutoTurnOff is running at path{path}");

                rkApp.SetValue(appName, path);
            }

            TimeCheck();
        }

        private static void SkipTimer()
        {
            while (true)
            {
                Console.ReadKey();
                ShutDownAt = ShutDownAt + 1;
            }
        }

        private static bool IsStartupItem()
        {

            if (rkApp.GetValue(appName) == null)
                return false;
            else
            {
                Console.WriteLine($"AutoTurnOff is running at path{rkApp.GetValue(appName)}");
                if (rkApp.GetValue(appName).ToString() == path)
                    return true;
                else
                    return false;
            }
        }

        private static void ShutDown()
        {
            while (secontsTillShutDown > 0)
            {
                Console.WriteLine($"AutoTurnOff is shutting down the PC in {secontsTillShutDown}");
                secontsTillShutDown--;
                Thread.Sleep(1000);
            }

            Console.WriteLine("AutoTurnOff is shutting down the PC!");
            Thread.Sleep(1000 * 2);
            Process.Start("shutdown", "/s /t 0");
        }

        private static void TimeCheck()
        {
            while (DateTime.Now.Hour != ShutDownAt)
            {
                Console.WriteLine($"It's not time yet {DateTime.Now.ToShortTimeString()}");
                Thread.Sleep(1000 * 60);
            }
            ShutDown();
        }
    }
}
