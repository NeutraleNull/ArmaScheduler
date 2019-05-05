using ArmaSheduler.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler
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
            SettingsFileReader reader = new SettingsFileReader();
            reader.ReadSettingsFile();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ArmaSheduler()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
