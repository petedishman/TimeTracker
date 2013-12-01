using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Core.DataCollection.UserActivity;

namespace TimeTracker.Core.DataCollection
{
    public class UserActivitySampleEventArgs : EventArgs
    {
        public UserActivitySampleEventArgs(UserActivityOverPeriod<ProcessInfo> processActivity)
        {
            this.ProcessActivity = processActivity;
        }

        public UserActivityOverPeriod<ProcessInfo> ProcessActivity { get; private set; }
    }

    public delegate void UserActivitySampleEventHandler(object sender, UserActivitySampleEventArgs e);

    public class UserActivityAggregator
    {
        public UserActivityAggregator(ProcessActivitySampler processActivitySampler)
        {
            // we need to fire off an event every five minutes to catch data from processActivitySampler
            // we then publish it in an event 

            this.processActivitySampler = processActivitySampler;

            this.activitySampleTimer = new Timer(OnActivitySampleTimer, null, 0, ActivitySamplePeriodMilliSeconds);
        }

        public event UserActivitySampleEventHandler UserActivitySampleReady;

        protected virtual void OnUserActivitySampleReady(UserActivitySampleEventArgs e)
        {
            if (UserActivitySampleReady != null)
            {
                UserActivitySampleReady(this, e);
            }
        }

        private void OnActivitySampleTimer(object info)
        {
            // grab stats from processactivitysampler and fire off the event
            var currentSegment = processActivitySampler.StartNewTimeSegment();

            var eventData = new UserActivitySampleEventArgs(currentSegment);

            OnUserActivitySampleReady(eventData);
        }

        ProcessActivitySampler processActivitySampler;
        
        private Timer activitySampleTimer;
        private const int ActivitySamplePeriodMilliSeconds = 5 * 60 * 1000;
    }
}
