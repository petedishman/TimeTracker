using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    public class UserActivitySampleEventArgs : EventArgs
    {
        public UserActivitySampleEventArgs()
        {

        }
    }

    public class ActivitySample
    {
        public class ActivitySample()
        {

        }

        public bool WasActivePeriod { get; private set; }

        public string PrimaryActivity { get; private set; }
        public int PrimaryActivityPercentage { get; private set; }


    }



    public delegate void UserActivitySampleEventHandler(object sender, UserActivitySampleEventArgs e);

    public class UserActivityAggregator
    {
        public UserActivityAggregator(ProcessActivitySampler processActivitySampler)
        {
            // we need to fire off an event every five minutes to catch data from processActivitySampler
            // we then publish it in an event 

            this.processActivitySampler = processActivitySampler;
        }

        public event UserActivitySampleEventHandler UserActivitySampleReady;

        protected virtual void OnUserActivitySampleReady(UserActivitySampleEventArgs e)
        {
            if (UserActivitySampleReady != null)
            {
                UserActivitySampleReady(this, e);
            }
        }

        ProcessActivitySampler processActivitySampler;
    }
}
