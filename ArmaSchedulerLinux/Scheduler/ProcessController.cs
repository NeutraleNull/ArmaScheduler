using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ArmaScheduler.Scheduler
{
    public class ProcessController
    {
        private readonly string _executable;
        private readonly string _parameter;
        private readonly Process _process;
        private bool _stopped;

        public ProcessController(string executable, string parameter)
        {
            this._executable = executable;
            this._parameter = parameter;
            _process = new Process
            {
                StartInfo = new ProcessStartInfo(executable, parameter)
            };
            _process.EnableRaisingEvents = true;
            _process.Exited += Process_Exited;
            _stopped = true;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (_stopped) return;
            Console.WriteLine("Process has crashed... Will restart soon");
            Task.Delay(1000 * 2).Wait();
            Start();
        }

        public void Restart()
        {
            Console.WriteLine("Restarting server!");
            Stop();
            Task.Delay(1000 * 5).Wait();
            Start();
        }

        public void Stop()
        {
            Console.WriteLine("Stopping server!");
            _stopped = true;
            _process.Kill();
        }

        public void Start()
        {
            Console.WriteLine("Starting server!");
            try
            {
                _process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start server, see more:\n{ex}");
            }
            _stopped = false;
        }
    }
}
