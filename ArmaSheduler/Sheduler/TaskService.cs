using System;
using System.Collections.Generic;
using System.Threading;

namespace ArmaSheduler.Sheduler
{
    public class TaskService
    {
        private static TaskService taskService;
        private List<Timer> timers = new List<Timer>();

        public static TaskService Instance => taskService ?? (taskService = new TaskService());

        private TaskService()
        {

        }

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }
            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }
            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));
            timers.Add(timer);
        }
    }
}
