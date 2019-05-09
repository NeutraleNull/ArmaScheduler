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
        private static RconConnector connector;
        private Settings settings;
        private BattlEyeClient client;
        private readonly object _lock = new object();

        private RconConnector()
        {
            client.BattlEyeDisconnected += Client_BattlEyeDisconnected;
            client.BattlEyeConnected += Client_BattlEyeConnected;
        }
            
        private void Client_BattlEyeConnected(BattlEyeConnectEventArgs args)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Connection to the server has been lost...\n Will try to reconnect now!");
                for (int tries = 0; tries < settings.repeat; tries++)
                {
                    Console.WriteLine($"Try {tries}/{settings.repeat}");
                    int result = OpenConnection();
                    if (result == 0)
                    {
                        Console.WriteLine("Connection reastablished...");
                        return;
                    }
                    Console.WriteLine("Connection could not be reastablished...");
                    Task.Delay(1000 * 2);
                }
                Environment.Exit(1);
            });
        }

        private void Client_BattlEyeDisconnected(BattlEyeDisconnectEventArgs args)
        {
            Console.WriteLine("Connection reastablished");
        }

        public static RconConnector GetRconConnector()
        {
            if(connector == null)
            {
                connector = new RconConnector();
            }
            return connector;
        }
        
        public void SetSettingsFile(Settings settings)
        {
            this.settings = settings;
        }

        public int OpenConnection()
        {
            IPAddress.TryParse(settings.ip, out IPAddress address);
            BattlEyeLoginCredentials credentials = new BattlEyeLoginCredentials
            {
                Host = address,
                Port = settings.port,
                Password = settings.password
            };
            lock (_lock)
            {
                client = new BattlEyeClient(credentials)
                {
                    ReconnectOnPacketLoss = true
                };
                var result = client.Connect();
                if (result != 0)
                {
                    Console.WriteLine("Connection to server failed");
                }
                return (int)result;
            }
        }

        public void SendCommand(string command)
        {
            lock(_lock)
            {
                if(client.Connected)
                    client.SendCommand(command, true);
                else
                    Console.WriteLine("Cannot send command... Not connected to server");
            }
        }

        public void CloseConnection()
        {
            client.Disconnect();
        }
    }
}
