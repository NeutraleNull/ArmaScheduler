using System.Collections.Generic;
using ArmaScheduler.Models;

namespace ArmaScheduler.Scheduler
{
    public class ArmaServer
    {
        private static ArmaServer _armaServer;
        private readonly List<ProcessController> _controller;
        private Settings _settings;

        private ArmaServer()
        {
            _controller = new List<ProcessController>();
        }

        public static ArmaServer GetInstance()
        {
            return _armaServer ?? (_armaServer = new ArmaServer());
        }

        public void SetSettingsFile(Settings settings)
        {
            this._settings = settings;
        }

        public void SetupServer()
        {
            _controller.Add(new ProcessController(_settings.serverExecutable, _settings.serverParameter));
            for (int i = 0; i < _settings.hcCount; i++)
            {
                _controller.Add(new ProcessController(_settings.serverExecutable, _settings.hcParameter));
            }
        }

        public void StartAll()
        {
            _controller.ForEach(x => x.Start());
        }

        public void StopAll()
        {
            _controller.ForEach(x => x.Stop());
        }

        public void RestartAll()
        {
            _controller.ForEach(x => x.Restart());
        }
    }
}
