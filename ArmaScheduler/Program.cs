using ArmaScheduler.Parser;
using System;
using ArmaScheduler.Exceptions;
using ArmaScheduler.Scheduler;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Owin.Hosting;

namespace ArmaScheduler
{
    static class Programs
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(String[] args)
        {
            _ = GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseMemoryStorage();
            MemoryStorageOptions options = new MemoryStorageOptions();
           // options.JobExpirationCheckInterval = TimeSpan.FromSeconds(30);
           // options.FetchNextJobTimeout = TimeSpan.FromSeconds(30);
          

            JobStorage.Current = new MemoryStorage(options);

            string baseAddress = "http://localhost:9000/";
            WebApp.Start<Startup>(url: baseAddress);

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
                rcon.StartQueueWorker();

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
