namespace ArmaSheduler.models
{
    class RepeatingService
    {
        public int startupDelay;
        public int repeating; //-1 means loop
        public int delay; //delay between executions
        public string rconCommand;
        public ExecutionTasks executeTask;
    }
}
