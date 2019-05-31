using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaScheduler.Scheduler
{
    public class SchedulerServer
    {
        public readonly BackgroundJobServer backgroundJobServer = null;

        private static SchedulerServer server = null;

        private SchedulerServer()
        {
            backgroundJobServer = new BackgroundJobServer();
        }

        public static SchedulerServer GetInstance()
        {
            if (server == null)
            {
                server = new SchedulerServer();
                return server;
            }
            else
                return server;
        }

        
    }
}
