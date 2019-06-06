using ArmaScheduler.Models;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ArmaScheduler.Scheduler
{
    public class TaskService
    {
        public void ExecuteRcon(string command)
        {
            var rcon = RconConnector.GetRconConnector();
            rcon.AddCommandToQueue(command);
        }

        public void RecurringRcon(RepeatingService item)
        {
            ExecuteRcon(item.rconCommand);
            if (item.repeating == -1)
            {
                BackgroundJob.Schedule<TaskService>(x => x.RecurringRcon(item), TimeSpan.FromMinutes(item.delay));
                return;
            }
            if (item.repeating == 0) return;
            if (item.repeating > 0)
            {
                item.repeating--;
                BackgroundJob.Schedule<TaskService>(x => x.RecurringRcon(item), TimeSpan.FromMinutes(item.delay));
            }
        }

        public void ReccuringRestart(RepeatingService item)
        {
            RestartServer(item.executeTask);
            if(item.repeating == -1)
            {
                BackgroundJob.Schedule<TaskService>(x => x.ReccuringRestart(item), TimeSpan.FromMinutes(item.delay));
                return;
            }
            if (item.repeating == 0) return;
            if (item.repeating > 0)
            {
                item.repeating--;
                BackgroundJob.Schedule<TaskService>(x => x.ReccuringRestart(item), TimeSpan.FromMinutes(item.delay));
            }
        }

        public void RestartServer(ExecutionTasks executionTasks)
        {
            switch (executionTasks)
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
        }
    }
}
