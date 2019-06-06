using System;
using System.Net;
using System.Threading.Tasks;
using ArmaSheduler.BattleNET;
using ArmaScheduler.Models;
using System.Collections.Generic;

namespace ArmaScheduler.Scheduler
{
    public class RconConnector
    {
        private static RconConnector _connector;
        private Settings _settings;
        private BattlEyeClient _client;
        private Queue<string> queue = new Queue<string>();
        private readonly object _lock = new object();
        private readonly object _lockQueue = new object();
        private bool _queueWorker = false;
        private bool _busy = false;

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
                _busy = true;
                Console.WriteLine("Connection to the server has been lost...\n Will try to reconnect now!");
                for (int tries = 0; tries < _settings.repeat; tries++)
                {
                    Console.WriteLine($"Try {tries}/{_settings.repeat}");
                    int result = OpenConnection();
                    if (result == 0)
                    {
                        Console.WriteLine("Connection reestablished...");
                        _busy = false;
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
            _busy = true;
            lock (_lock)
            {
                var result = _client.Connect();
                if (result != 0)
                {
                    Task.Run(() =>
                    {
                        _busy = true;
                        Console.WriteLine("Connection to the server could not be established...\n Will try to reconnect now!");
                        for (int tries = 0; tries < _settings.repeat; tries++)
                        {
                            Console.WriteLine($"Try {tries}/{_settings.repeat}");
                            result = _client.Connect();
                            if (result == 0)
                            {
                                Console.WriteLine("Connection established...");
                                _busy = false;
                                return;
                            }
                            Console.WriteLine("Connection could not be established...");
                            Task.Delay(5 * 1000).Wait();
                        }
                        Environment.Exit(1);
                    });
                }
                _busy = false;
                return (int)result;
            }
        }

        private void SendCommand(string command)
        {
            if(_client.Connected)
            {
                lock(_lock)
                {
                    try
                    {
                        _client.SendCommand(command);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Something went wrong: {ex}");
                    }
                }
            }
            else
            {
                OpenConnection();
                SendCommand(command);
            }
        }

        public void AddCommandToQueue(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException("message", nameof(command));
            }
            lock (_lockQueue)
            {
                queue.Enqueue(command);
            }
        }

        public void StartQueueWorker()
        {
            if (_queueWorker) return;
            _queueWorker = true;
            Task.Run(() =>
            {
                while(true)
                {
                    if(queue.Count > 0)
                    {
                        string command = "";
                        lock(_lockQueue)
                        {
                            command = queue.Dequeue();
                        }
                        if(!_busy)
                        {
                            Console.WriteLine("Sending command: "+command);
                            _client.SendCommand(command);
                        }
                        else
                        {
                            Console.WriteLine("RCON currently busy... delaying command");
                            lock(_lockQueue)
                            {
                                queue.Enqueue(command);
                            }
                        }
                    }
                    Task.Delay(2000).Wait();
                }
            });

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
