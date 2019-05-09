using ArmaScheduler.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ArmaScheduler
{
    static class Programs
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {
            AllocConsole();
            //SettingsFileReader reader = new SettingsFileReader();
            //reader.ReadSettingsFile();
            var servicesToRun = new ServiceBase[]
            {
                new ArmaScheduler()
            };
            ServiceBase.Run(servicesToRun);
            Console.WriteLine("Press any key to kill the console");
            Console.ReadKey();
        }
    }
}
