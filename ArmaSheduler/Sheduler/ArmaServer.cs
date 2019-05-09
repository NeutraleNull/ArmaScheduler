using ArmaSheduler.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler.Sheduler
{
    public class ArmaServer
    {
        private static ArmaServer armaServer;
        private List<ProcessController> controller;
        private Settings settings;

        private ArmaServer()
        {
            controller = new List<ProcessController>();
        }

        public static ArmaServer GetInstance()
        {
            if (armaServer == null)
                armaServer = new ArmaServer();
            return armaServer;
        }

        public void SetSettingsFile(Settings settings)
        {
            this.settings = settings;
        }

        public void SetupServer()
        {
            controller.Add(new ProcessController(settings.serverExecutable, settings.serverParameter));
            for (int i = 0; i < settings.hcCount; i++)
            {
                controller.Add(new ProcessController(settings.serverExecutable, settings.hcParameter));
            }
        }

        public void StartAll()
        {
            controller.ForEach(x => x.Start());
        }

        public void StopAll()
        {
            controller.ForEach(x => x.Stop());
        }

        public void RestartAll()
        {
            controller.ForEach(x => x.Restart());
        }
    }
}
