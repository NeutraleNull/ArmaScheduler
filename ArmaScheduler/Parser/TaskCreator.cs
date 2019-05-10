using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaScheduler.Models;
using ArmaScheduler.Scheduler;

namespace ArmaScheduler.Parser
{
    public static class TaskCreator
    {
        public static void CreateTasks(JsonModel model)
        {
            foreach (var item in model.scheduledTasks)
            {
                if (item.rconCommand != null)
                {
                    TaskScheduler.IntervalInDays(item.time.Hours, item.time.Minutes, 1, () =>
                    {
                        var rcon = RconConnector.GetRconConnector();
                        rcon.SendCommand(item.rconCommand);
                    });
                }
                if(item.executeTask != ExecutionTasks.none)
                {
                    TaskScheduler.IntervalInDays(item.time.Hours, item.time.Minutes, 1, () =>
                    {
                        switch (item.executeTask)
                        {
                            case ExecutionTasks.start:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.StartAll();
                                break;
                            }
                            case ExecutionTasks.stop:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.StopAll();
                                break;
                            }
                            case ExecutionTasks.restart:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.RestartAll();
                                break;
                            }
                        }
                    });
                }
            }
     
            foreach (var item in model.repeatingServices)
            {
                var time = DateTime.Now;
                if (item.rconCommand != null)
                {
                    
                    TaskScheduler.IntervalInMinutes(time.Hour, time.Minute + item.startupDelay, item.delay, () =>
                    {
                        var rcon = RconConnector.GetRconConnector();
                        rcon.SendCommand(item.rconCommand);
                    });
                }
                if (item.executeTask != ExecutionTasks.none)
                {
                    TaskScheduler.IntervalInMinutes(time.Hour, time.Minute + item.startupDelay, item.delay, () =>
                    {
                        switch (item.executeTask)
                        {
                            case ExecutionTasks.start:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.StartAll();
                                break;
                            }
                            case ExecutionTasks.stop:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.StopAll();
                                break;
                            }
                            case ExecutionTasks.restart:
                            {
                                var armaServer = ArmaServer.GetInstance();
                                armaServer.RestartAll();
                                break;
                            }
                        }
                    });
                }
            }
        }
    }
}
