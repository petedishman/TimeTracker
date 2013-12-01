using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection
{
    // is it possible to create a generic version of the below
    // so I could use it for the UserHint stuff too
    // that would be periodically grabbing what I'd typed in to a text box
    // or an issue key


    public class UserActivityOverPeriod<T> where T : IEqualish<T>
    {
        public UserActivityOverPeriod()
        {
            ActivitySamples = new List<UserActivity<T>>();
        }

        public int TotalSecondsInSegment
        {
            get
            {
                return ActivitySamples.Sum(x => x.Seconds);
            }
        }

        public bool IsActiveSegment
        {
            get
            {
                UserActivity<T> primarySample = (from sample in ActivitySamples
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

        public T PrimaryActivityInSegment
        {
            get
            {
                UserActivity<T> primarySample = (from sample in ActivitySamples
                                                 where sample.WasActive
                                                 orderby sample.Seconds descending
                                                 select sample).First();
                if (primarySample == null)
                {
                    return default(T);
                }
                else
                {
                    return primarySample.Details;
                }
            }
        }

        public void AddInactiveSample()
        {
            // find an existing inactive sample in the list and increment the seconds count
            var inactiveSample = ActivitySamples.FirstOrDefault(x => !x.WasActive);

            if (inactiveSample == null)
            {
                ActivitySamples.Add(new UserActivity<T>()
                {
                    Seconds = 1,
                    WasActive = false,
                    Details = default(T)
                });
            }
            else
            {
                inactiveSample.Seconds++;
            }
        }

        public void AddActiveSample(T details)
        {
            // see if there's a matching sample for this process and increment the seconds count
            var matchingSample = ActivitySamples.FirstOrDefault(x => x.Details != null && x.Details.IsEqualishTo(details));

            if (matchingSample == null)
            {
                ActivitySamples.Add(new UserActivity<T>()
                {
                    Seconds = 1,
                    WasActive = true,
                    Details = details
                });
            }
            else
            {
                matchingSample.Seconds++;
            }
        }

        public IReadOnlyList<UserActivity<T>> Samples
        {
            get
            {
                return ActivitySamples.AsReadOnly();
            }
        }

        private List<UserActivity<T>> ActivitySamples;
    }

    /*
    public class ProcessActivityTimeSegment
    {
        public ProcessActivityTimeSegment()
        {
            UserActivitySamples = new List<ProcessActivitySample>();
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
                ProcessActivitySample primarySample = (from sample in UserActivitySamples
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
                ProcessActivitySample primarySample = (from sample in UserActivitySamples
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
                UserActivitySamples.Add(new ProcessActivitySample()
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
            var matchingSample = UserActivitySamples.FirstOrDefault(x => x.ProcessInfo != null && x.ProcessInfo.Equals(processInfo));

            if (matchingSample == null)
            {
                UserActivitySamples.Add(new ProcessActivitySample()
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

        public IReadOnlyList<ProcessActivitySample> Samples
        {
            get
            {
                return UserActivitySamples.AsReadOnly();
            }
        }

        private List<ProcessActivitySample> UserActivitySamples { get; set; }
    }
    */
}
