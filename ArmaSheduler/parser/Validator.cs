using ArmaSheduler.models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler.parser
{
    public static class Validator
    {
        static void ValidateJsonModel(ref JsonModel model)
        {
            Console.WriteLine("Scanning settings!");
            if (!File.Exists(model.settings.serverExecutable))
            {
                Console.WriteLine("Fatal error! server executable not found!");
                Environment.Exit(1);
            }
            Console.WriteLine($"Server params: {model.settings.serverParameter}");
            Console.WriteLine($"Headless client count: {model.settings.hcCount}");
            Console.WriteLine($"Headless client params: {model.settings.hcParameter}");
            Console.WriteLine($"Reapting {model.settings.repeat} times until terminate");

            Console.WriteLine("Scanning ");
        }
    }
}
