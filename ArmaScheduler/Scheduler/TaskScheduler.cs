using System;

namespace ArmaScheduler.Scheduler
{
    public static class TaskScheduler
    {
        public static void IntervalInSeconds(int hour, int sec, double interval, Action task)
        {
            interval = interval / 3600;
            TaskService.Instance.ScheduleTask(hour, sec, interval, task);
        }
        public static void IntervalInMinutes(int hour, int min, double interval, Action task)
        {
            interval = interval / 60;
            TaskService.Instance.ScheduleTask(hour, min, interval, task);
        }
        public static void IntervalInHours(int hour, int min, double interval, Action task)
        {
            TaskService.Instance.ScheduleTask(hour, min, interval, task);
        }
        public static void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            TaskService.Instance.ScheduleTask(hour, min, interval, task);
        }

        public static void KillAllTasks()
        {
            
        }
    }
}
