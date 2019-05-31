using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaScheduler.Models;
using ArmaScheduler.Scheduler;
using Hangfire;

namespace ArmaScheduler.Parser
{
    public static class TaskCreator
    {
        public static void CreateTasks(JsonModel model)
        {
            //var server = SchedulerServer.GetInstance();
            _ = BackgroundJob.Enqueue(() => Console.WriteLine("Starting scheduler..."));

            foreach (var item in model.scheduledTasks)
            {
                if (item.rconCommand != null)
                {
                    RecurringJob.AddOrUpdate<TaskService>(x => x.ExecuteRcon(item.rconCommand), string.Format("{0} {1} * * *", item.time.Minutes, item.time.Hours), TimeZoneInfo.Local);
                }
                if (item.executeTask != ExecutionTasks.none)
                {
                    RecurringJob.AddOrUpdate<TaskService>(x => x.RestartServer(item.executeTask), string.Format("{0} {1} * * *", item.time.Minutes, item.time.Hours), TimeZoneInfo.Local);
                }
            }
     
            foreach (var item in model.repeatingServices)
            {
                var time = DateTime.Now;
                if (item.rconCommand != null)
                {
                    BackgroundJob.Schedule<TaskService>(x => x.RecurringRcon(item), TimeSpan.FromMinutes(item.startupDelay));
                }
                if (item.executeTask != ExecutionTasks.none)
                {
                    BackgroundJob.Schedule<TaskService>(x => x.RestartServer(item.executeTask), TimeSpan.FromMinutes(item.startupDelay));
                }
            }
        }
    }
}
