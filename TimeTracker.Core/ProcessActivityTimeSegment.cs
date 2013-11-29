using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    // is it possible to create a generic version of the below
    // so I could use it for the UserHint stuff too
    // that would be periodically grabbing what I'd typed in to a text box
    // or an issue key
    
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
}
