using ArmaSheduler.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleNET;
using System.Net;

namespace ArmaSheduler.Sheduler
{
    public class RconConnector
    {
        private static readonly RconConnector connector = new RconConnector();
        private Settings settings;
        private BattlEyeClient client;

        private RconConnector()
        {

        }

        public RconConnector GetRconConnector()
        {
            return connector;
        }
        
        public void SetSettingsFile(Settings settings)
        {
            this.settings = settings;
        }

        public void OpenConnection()
        {
            IPAddress address;
            IPAddress.TryParse(settings.ip, out address);
            BattlEyeLoginCredentials credentials = new BattlEyeLoginCredentials
            {
                Host = address,
                Port = settings.port,
                Password = settings.password
            };
            lock (this)
            {
                client = new BattlEyeClient(credentials);
                client.ReconnectOnPacketLoss = true;
                var result = client.Connect();
                if (result != 0)
                {
                    Console.WriteLine("Connection to server failed");
                }
            }
        }

        public void SendCommand(string command)
        {
            lock(this)
            {
                if(!client.Connected)
                {
                    OpenConnection();
                }
                client.SendCommand(command, true);
            }
        }
    }
}
