using ArmaSheduler.models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ArmaSheduler.parser
{
    public static class Validator
    {
        public static void ValidateJsonModel(ref JsonModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Console.WriteLine("Scanning settings!\n");
            if (!File.Exists(model.settings.serverExecutable))
            {
                Console.WriteLine("Fatal error! server executable not found!");
                //Environment.Exit(1);
            }
            Console.WriteLine($"Server params: {model.settings.serverParameter}");
            Console.WriteLine($"Server IP: {model.settings.ip}:{model.settings.port}");
            Console.WriteLine($"Headless client count: {model.settings.hcCount}");
            Console.WriteLine($"Headless client params: {model.settings.hcParameter}");
            if (model.settings.repeat == 0) model.settings.repeat = 100;
            Console.WriteLine($"Reapting {model.settings.repeat} times until terminate");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Scanning shedule tasks ");
            if(model.sheduledTasks == null) Console.WriteLine("No shedule tasks found!");
            foreach (var item in model.sheduledTasks)
            {
                Console.WriteLine($"Task at {item.time.ToString()}\n#RCON command: {item.rconCommand}\nExecute task: {item.executeTask.ToString()}\n");
            }
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Scanning repeating tasks\n");
            if (model.repeatingServices == null) Console.WriteLine("No repeating services found!");
            foreach (var item in model.repeatingServices)
            {
                Console.WriteLine($"Task delayed with {item.startupDelay}\n#RCON command:{item.rconCommand}\nExecute task: {item.executeTask.ToString()}\nReapting: {item.repeating} with {item.delay} seconds interval\n");
            }
            Thread.Sleep(60 * 1000);
        }
    }
}
