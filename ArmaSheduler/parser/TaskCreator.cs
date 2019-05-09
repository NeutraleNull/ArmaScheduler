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
                        if (item.executeTask == ExecutionTasks.start)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.StartAll();
                        }
                        if (item.executeTask == ExecutionTasks.stop)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.StopAll();
                        }
                        if (item.executeTask == ExecutionTasks.restart)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.RestartAll();
                        }
                    });
                }
            }
     
            foreach (var item in model.repeatingServices)
            {
                if (item.rconCommand != null)
                {
                    TaskScheduler.IntervalInMinutes(DateTime.Now.Hour, DateTime.Now.Minute + item.startupDelay, item.delay, () =>
                    {
                        var rcon = RconConnector.GetRconConnector();
                        rcon.SendCommand(item.rconCommand);
                    });
                }
                if (item.executeTask != ExecutionTasks.none)
                {
                    TaskScheduler.IntervalInMinutes(DateTime.Now.Hour, DateTime.Now.Minute + item.startupDelay, item.delay, () =>
                    {
                        if (item.executeTask == ExecutionTasks.start)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.StartAll();
                        }
                        if (item.executeTask == ExecutionTasks.stop)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.StopAll();
                        }
                        if (item.executeTask == ExecutionTasks.restart)
                        {
                            var armaServer = ArmaServer.GetInstance();
                            armaServer.RestartAll();
                        }
                    });
                }
            }
        }
    }
}
