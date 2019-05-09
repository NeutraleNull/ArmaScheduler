using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler.Sheduler
{
    public class ProcessController
    {
        private readonly string executable;
        private readonly string parameter;
        private Process process;
        private bool stopped;

        public ProcessController(string executable, string parameter)
        {
            this.executable = executable;
            this.parameter = parameter;
            process = new Process
            {
                StartInfo = new ProcessStartInfo(executable, parameter)
            };
            process.Exited += Process_Exited;
            stopped = true;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (stopped) return;
            Console.WriteLine("Process has crashed... Will restart soon");
            Task.Delay(1000 * 2);
            Start();
        }

        public void Restart()
        {
            Stop();
            Task.Delay(1000 * 2);
            Start();
        }

        public void Stop()
        {
            stopped = true;
            process.Kill();
        }

        public void Start()
        {
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start server, see more:\n{ex}");
            }
            stopped = false;
        }
    }
}
