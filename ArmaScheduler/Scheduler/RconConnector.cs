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
            
        }
            
        private void Client_BattlEyeConnected(BattlEyeConnectEventArgs args)
        {
            lock (_lock)
            {
                if(_client.Connected)
                    Console.WriteLine("Connection reestablished");
            }
        }

        private void Client_BattlEyeDisconnected(BattlEyeDisconnectEventArgs args)
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
                        Console.WriteLine("Connection reestablished...");
                        return;
                    }
                    Console.WriteLine("Connection could not be reestablished...");
                    Task.Delay(5000);
                }
                Environment.Exit(1);
            });
        }

        public static RconConnector GetRconConnector()
        {
            return _connector ?? (_connector = new RconConnector());
        }
        
        public void SetSettingsFile(Settings settings)
        {
            this._settings = settings;
            IPAddress.TryParse(_settings.ip, out IPAddress address);
            var credentials = new BattlEyeLoginCredentials
            {
                Host = address,
                Port = _settings.port,
                Password = _settings.password
            };
            _client = new BattlEyeClient(credentials)
            {
                ReconnectOnPacketLoss = true
            };
            _client.BattlEyeDisconnected += Client_BattlEyeDisconnected;
            _client.BattlEyeConnected += Client_BattlEyeConnected;
        }

        public int OpenConnection()
        {
            lock (_lock)
            {
                var result = _client.Connect();
                if (result != 0)
                {
                    Console.WriteLine("Connection to server failed");
                }

                Task.Run(() =>
                {
                    Console.WriteLine("Connection to the server has been lost...\n Will try to reconnect now!");
                    for (int tries = 0; tries < _settings.repeat; tries++)
                    {
                        Console.WriteLine($"Try {tries}/{_settings.repeat}");
                        result = _client.Connect();
                        if (result == 0)
                        {
                            Console.WriteLine("Connection established...");
                            return;
                        }
                        Console.WriteLine("Connection could not be established...");
                        Task.Delay(5 *1000).Wait();
                    }
                    Environment.Exit(1);
                });
                return (int)result;
            }
        }

        public void SendCommand(string command)
        {
            lock(_lock)
            {
                if (_client.Connected)
                    _client.SendCommand(command, true);
                else
                {
                    OpenConnection();
                    if (_client.Connected)
                        _client.SendCommand(command, true);
                    else
                        Console.WriteLine("Cannot send command... Not connected to server");
                }
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
