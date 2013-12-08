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

        public int SecondsInPrimarySegment
        {
            get
            {
                var primarySample = (from sample in ActivitySamples
                                     orderby sample.Seconds descending
                                     select sample).FirstOrDefault();

                if (primarySample == null)
                {
                    return 0;
                }
                else
                {
                    return primarySample.Seconds;
                }
            }
        }

        public bool IsActiveSegment
        {
            get
            {
                UserActivity<T> primarySample = (from sample in ActivitySamples
                                                 orderby sample.Seconds descending
                                                 select sample).FirstOrDefault();

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
                                                 orderby sample.Seconds descending
                                                 select sample).FirstOrDefault();
                if (primarySample == null)
                {
                    return default(T);
                }
                else
                {
                    if (primarySample.WasActive)
                    {
                        return primarySample.Details;
                    }
                    else
                    {
                        return default(T);
                    }
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
            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

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
}
