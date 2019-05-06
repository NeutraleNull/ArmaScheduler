using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler.models
{
    public class JsonModel
    {
        public Settings settings;
        public Shedule[] sheduledTasks;
        public RepeatingService[] repeatingServices;
    }
}
