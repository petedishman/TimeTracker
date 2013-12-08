using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Core;

namespace TimeTracker.UI
{
    class TimeTrackerAgent
    {
        public TimeTrackerAgent(DataLogger dataLogger)
        {
            this.dataLogger = dataLogger;
        }

        private DataLogger dataLogger;
    }
}
