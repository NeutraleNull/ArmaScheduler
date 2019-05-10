using ArmaScheduler.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Configuration.Install;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ArmaScheduler.Exceptions;
using ArmaScheduler.Scheduler;

namespace ArmaScheduler
{
    static class Programs
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(String[] args)
        {
            try
            {
                SettingsFileReader reader = new SettingsFileReader();
                var settingsFile = reader.ReadSettingsFile();
                Validator.ValidateJsonModel(ref settingsFile);

                var armaServer = ArmaServer.GetInstance();
                armaServer.SetSettingsFile(settingsFile.settings);
                armaServer.SetupServer();
                

                var rcon = RconConnector.GetRconConnector();
                rcon.SetSettingsFile(settingsFile.settings);
                rcon.OpenConnection();

                TaskCreator.CreateTasks(settingsFile);
                armaServer.StartAll();

                while (true)
                {
                    Console.ReadKey();
                }
            }
            catch (SettingsFileReadException e)
            {
                Console.WriteLine("Error reading the settings file...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}
