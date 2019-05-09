using ArmaSheduler.parser;
using ArmaSheduler.Sheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler
{
    public partial class ArmaSheduler : ServiceBase
    {
        public ArmaSheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SettingsFileReader reader = new SettingsFileReader();
            var settingsFile = reader.ReadSettingsFile();
            Validator.ValidateJsonModel(ref settingsFile);

            var armaServer = ArmaServer.GetInstance();
            armaServer.SetSettingsFile(settingsFile.settings);
            armaServer.SetupServer();

            var rcon = RconConnector.GetRconConnector();
            rcon.OpenConnection();

            TaskCreator.CreateTasks(settingsFile);
        }

        protected override void OnStop()
        {
            var armaServer = ArmaServer.GetInstance();
            armaServer.StopAll();
        }
    }
}
