using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    /* Need a class that I can give a ActiveProcessHelper and a UserActivityMonitor too
     * The class will then fire every second? and find out if the user now counts as away
     * or if not the current process they're using.
     * We'll then record all of that info as we go.
     * And give it up when asked
     */

    /* Need a data structure to store all of this in that works no matter when someone asks us
     * for the data.
     * So we should store the data indexed on TimeSegment (5 minute segments)
     * when we have data we can just add it to whatever the current segment is
     */

    public class UserActivityTimeSegment
    {
        public UserActivityTimeSegment()
        {
            UserActivitySamples = new List<UserActivitySample>();
        }

        public int TotalSecondsInSegment
        {
            get
            {
                return UserActivitySamples.Sum(x => x.Seconds);
            }
        }

        public bool IsActiveSegment
        {
            get
            {
                UserActivitySample primarySample = (from sample in UserActivitySamples
                                                    orderby sample.Seconds descending
                                                    select sample).First();

                if (primarySample == null)
                {
                    // no samples, that counts as inactive
                    return false;
                }
                else
                {
                    return primarySample.WasActive;
                }
            }
        }

        public ProcessInfo PrimaryActivityInSegment
        {
            get
            {
                UserActivitySample primarySample = (from sample in UserActivitySamples
                                                    where sample.WasActive
                                                    orderby sample.Seconds descending
                                                    select sample).First();
                if (primarySample == null)
                {
                    return null;
                }
                else
                {
                    return primarySample.ProcessInfo;
                }
            }
        }

        public void AddInactiveSample()
        {
            // find an existing inactive sample in the list and increment the seconds count
            var inactiveSample = UserActivitySamples.FirstOrDefault(x => !x.WasActive);

            if (inactiveSample == null)
            {
                UserActivitySamples.Add(new UserActivitySample()
                                            {
                                                Seconds = 1,
                                                WasActive = false,
                                                ProcessInfo = null
                                            });
            }
            else
            {
                inactiveSample.Seconds++;
            }
        }

        public void AddActiveSample(ProcessInfo processInfo)
        {
            // see if there's a matching sample for this process and increment the seconds count
            var matchingSample = UserActivitySamples.FirstOrDefault(x => x.ProcessInfo.Equals(processInfo));

            if (matchingSample == null)
            {
                UserActivitySamples.Add(new UserActivitySample()
                                            {
                                                Seconds = 1,
                                                WasActive = true,
                                                ProcessInfo = processInfo
                                            });
            }
            else
            {
                matchingSample.Seconds++;
            }
        }

        public IReadOnlyList<UserActivitySample> Samples
        {
            get
            {
                return UserActivitySamples.AsReadOnly();
            }
        }

        private List<UserActivitySample> UserActivitySamples { get; set; }
    }

    public class UserActivitySample
    {
        public int Seconds { get; set; }
        public bool WasActive { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
    }


    // This class takes the two monitor classes
    // and fires every n milliseconds. It then samples what the user is doing
    // before adding that to a list that will average it out 
    public class UserActivitySampler
    {
        public UserActivitySampler(IUserActivityMonitor userActivityMonitor, IActiveProcessHelper activeProcessHelper)
        {
            this.userActivityMonitor = userActivityMonitor;
            this.activeProcessHelper = activeProcessHelper;

            UserInactiveAfterNSeconds = DefaultUserInactivityTimeSeconds;

            activitySampleTimer = new Timer(OnActivitySampleTimer, null, 0, ActivitySamplePeriodMilliSeconds);

            this.timeSegment = new UserActivityTimeSegment();
            TrackingPaused = false;
        }   

        public UserActivityTimeSegment StartNewTimeSegment()
        {
            // I probably need to lock access to this.timeSegment while we do this.
            UserActivityTimeSegment oldTimeSegment;
            lock (lockObject)
            {
                oldTimeSegment = this.timeSegment;
                this.timeSegment = new UserActivityTimeSegment();
            }

            return oldTimeSegment;
        }

        public bool TrackingPaused { get; set; }
        private UserActivityTimeSegment timeSegment;

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
