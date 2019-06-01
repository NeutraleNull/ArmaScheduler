using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaScheduler.Exceptions;
using Newtonsoft.Json;
using System.Threading;
using ArmaScheduler.Models;

namespace ArmaScheduler.Parser
{
    public class SettingsFileReader
    {
        private static readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "setting.json");
        
        public JsonModel ReadSettingsFile()
        {
            var file = new JsonModel();
            if (File.Exists(_path))
            {
                using (var reader = new StreamReader(_path))
                {
                    try
                    {
                        file = JsonConvert.DeserializeObject<JsonModel>(reader.ReadToEnd());
                        Validator.ValidateJsonModel(ref file);
                    }
                    catch
                    {
                        throw new SettingsFileReadException();
                    }
                }
                return file;
            }
            else
            {
                Console.WriteLine("Settings File not found! generating base layout...");
                file.repeatingServices = new RepeatingService[2];
                file.repeatingServices[0] = new RepeatingService
                {
                    startupDelay = 10,
                    delay = 900,
                    repeating = -1,
                    rconCommand = "say -1 Hallo Welt"
                };
                file.repeatingServices[1] = new RepeatingService
                {
                    startupDelay = 60 * 60,
                    repeating = 1,
                    rconCommand = "#shutdown"
                };
                file.scheduledTasks = new Schedule[2];
                file.scheduledTasks[0] = new Schedule
                {
                    time = TimeSpan.Parse("10:00:00"),
                    executeTask = ExecutionTasks.restart,
                    rconCommand = "say -1 RESTART!"
                };
                file.scheduledTasks[1] = new Schedule
                {
                    time = TimeSpan.Parse("12:30:00"),
                    rconCommand = "say -1 Restart will be in 30 min"
                };
                file.settings = new Settings
                {
                    hcCount = 3,
                    serverExecutable = "D:\\Arma3\\arma3server_64.exe",
                    hcParameter = "-client -connect=127.0.0.1 -port=2302 -autoinit -bla -da",
                    serverParameter = "-cpuCount=4 -autoinit",
                    repeat = 100,
                    ip = "127.0.0.1",
                    port = 2307,
                    password = "iamnotanrconpassword",
                    timeout = 100               
                };
                string json = JsonConvert.SerializeObject(file);
                using (var writer = new StreamWriter(_path, false, Encoding.UTF8))
                {
                    writer.Write(json);
                }
                Console.WriteLine("Template file has been generated to desktop");
                Console.WriteLine("Programm will terminate in 5 sec..");
                Thread.Sleep(1000 * 5);
                Environment.Exit(1);
            }
            return file;
        }
    }
}
