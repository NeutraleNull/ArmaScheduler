using ArmaScheduler.Exceptions;
using ArmaScheduler.Parser;
using ArmaScheduler.Scheduler;
using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.IO;
using System.Linq;

namespace ArmaScheduler
{
    class Program
    {
        static void Main(string[] args)
        { 
            if(args.Length < 0)
            {
                Console.WriteLine("No Arguments found! You need to specify a path to your settings.json");
                Environment.Exit(1);
            }

            var settingsFilePath = args.FirstOrDefault(x => x.Contains("-settingsfile=")).Replace("-settingsfile=","");
            if(settingsFilePath == null)
            {
                Console.WriteLine("You need to specify a path to your settings.json");
                Environment.Exit(1);
            }
            if (!File.Exists(settingsFilePath))
            {
                if (args.FirstOrDefault(x => x.Contains("-generateConfig")) != null)
                {
                    SettingsFileReader reader = new SettingsFileReader();
                    var settingsFile = reader.ReadSettingsFile(settingsFilePath, true);
                    Console.WriteLine("Config example has been generated to your specified path.\n Terminating...");
                    Environment.Exit(0);
                }
                Console.WriteLine("No settingsfile found please double check your path");
                Environment.Exit(1);
            }
           
            _ = GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseMemoryStorage();
            MemoryStorageOptions options = new MemoryStorageOptions();
            // options.JobExpirationCheckInterval = TimeSpan.FromSeconds(30);
            // options.FetchNextJobTimeout = TimeSpan.FromSeconds(30);


            JobStorage.Current = new MemoryStorage(options);

            string baseAddress = "http://localhost:9000/";
            //WebApp.Start<Startup>(url: baseAddress);

            try
            {
                SettingsFileReader reader = new SettingsFileReader();
                var settingsFile = reader.ReadSettingsFile(settingsFilePath);
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
