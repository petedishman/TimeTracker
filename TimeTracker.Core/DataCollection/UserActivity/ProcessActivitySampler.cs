using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection.UserActivity
{
    // This class takes the two monitor classes
    // and fires every n milliseconds. It then samples what the user is doing
    // before adding that to a list that will average it out 
    public class ProcessActivitySampler
    {
        public ProcessActivitySampler(IUserActivityMonitor userActivityMonitor, IActiveProcessHelper activeProcessHelper)
        {
            this.userActivityMonitor = userActivityMonitor;
            this.activeProcessHelper = activeProcessHelper;

            UserInactiveAfterNSeconds = DefaultUserInactivityTimeSeconds;

            activitySampleTimer = new Timer(OnActivitySampleTimer, null, 0, ActivitySamplePeriodMilliSeconds);

            this.timeSegment = new UserActivityOverPeriod<ProcessInfo>();
            TrackingPaused = false;
        }   

        public UserActivityOverPeriod<ProcessInfo> StartNewTimeSegment()
        {
            // I probably need to lock access to this.timeSegment while we do this.
            UserActivityOverPeriod<ProcessInfo> oldTimeSegment;
            lock (lockObject)
            {
                oldTimeSegment = this.timeSegment;
                this.timeSegment = new UserActivityOverPeriod<ProcessInfo>();
            }

            return oldTimeSegment;
        }

        public bool TrackingPaused { get; set; }
        private UserActivityOverPeriod<ProcessInfo> timeSegment;

        private IUserActivityMonitor userActivityMonitor;
        private IActiveProcessHelper activeProcessHelper;

        private const int DefaultUserInactivityTimeSeconds = 5 * 60;
        public int UserInactiveAfterNSeconds { get; set; }

        private Timer activitySampleTimer;
        private const int ActivitySamplePeriodMilliSeconds = 1000;

        private object lockObject = new Object();

        private void OnActivitySampleTimer(object info)
        {
            lock(lockObject)
            {
                if (TrackingPaused)
                {
                    // for now we'll treat paused tracking as being inactive?
                    // probably need to do something else really
                    timeSegment.AddInactiveSample();
                }
                else
                {
                    if (IsUserActive())
                    {
                        var activeProcess = activeProcessHelper.GetActiveProcess();
                        // what do we do with it?
                        timeSegment.AddActiveSample(activeProcess);
                    }
                    else
                    {
                        // ok so what do we do
                        timeSegment.AddInactiveSample();
                    }

                }
            }
        }

        private bool IsUserActive()
        {
            return userActivityMonitor.SecondsSinceLastUserActivity < UserInactiveAfterNSeconds;
        }
    }
}
