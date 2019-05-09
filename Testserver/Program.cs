using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I ama debug console\nlook at the arguments:\n---------------------------");
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("\nPress any key to close the programm");
            Console.ReadKey();
        }
    }
}
