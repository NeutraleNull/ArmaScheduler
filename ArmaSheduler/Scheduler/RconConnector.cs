using System;
using System.Net;
using System.Threading.Tasks;
using ArmaSheduler.BattleNET;
using ArmaScheduler.Models;

namespace ArmaScheduler.Scheduler
{
    public class RconConnector
    {
        private static RconConnector _connector;
        private Settings _settings;
        private BattlEyeClient _client;
        private readonly object _lock = new object();

        private RconConnector()
        {
            _client.BattlEyeDisconnected += Client_BattlEyeDisconnected;
            _client.BattlEyeConnected += Client_BattlEyeConnected;
        }
            
        private void Client_BattlEyeConnected(BattlEyeConnectEventArgs args)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Connection to the server has been lost...\n Will try to reconnect now!");
                for (int tries = 0; tries < _settings.repeat; tries++)
                {
                    Console.WriteLine($"Try {tries}/{_settings.repeat}");
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
            return _connector ?? (_connector = new RconConnector());
        }
        
        public void SetSettingsFile(Settings settings)
        {
            this._settings = settings;
        }

        public int OpenConnection()
        {
            IPAddress.TryParse(_settings.ip, out IPAddress address);
            var credentials = new BattlEyeLoginCredentials
            {
                Host = address,
                Port = _settings.port,
                Password = _settings.password
            };
            lock (_lock)
            {
                _client = new BattlEyeClient(credentials)
                {
                    ReconnectOnPacketLoss = true
                };
                var result = _client.Connect();
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
                if(_client.Connected)
                    _client.SendCommand(command, true);
                else
                    Console.WriteLine("Cannot send command... Not connected to server");
            }
        }

        public void CloseConnection()
        {
            lock (_lock)
            {
                _client?.Disconnect();
            }
        }
    }
}
