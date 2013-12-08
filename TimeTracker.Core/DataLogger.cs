using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Core.DataCollection;
using TimeTracker.Core.DataCollection.UserActivity;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.Core
{
    public class DataLogger
    {
        public DataLogger(IDataContext dataContext, IUserActivityAggregator activityAggregator, ITimeSegmentProvider timeSegmentProvider)
        {
            this.dataContext = dataContext;
            this.timeSegmentProvider = timeSegmentProvider;

            activityAggregator.UserActivitySampleReady += OnUserActivitySampleReady;            
        }

        void OnUserActivitySampleReady(object sender, UserActivitySampleEventArgs e)
        {
            // take the stats we just got and stick them in the database
            // for whatever the current time segment is

            // This is going to need some exception handling

            TimeSegment newTimeSegmentData = GetTimeSegmentData(e.ProcessActivity);

            this.dataContext.TimeSegments.Add(newTimeSegmentData);
            this.dataContext.Commit();
        }

        TimeSegment GetTimeSegmentData(UserActivityOverPeriod<ProcessInfo> userActivity)
        {
            var newTimeSegment = new TimeSegment();
            var currentTimeSegment = this.timeSegmentProvider.CurrentTimeSegment;

            newTimeSegment.StartOfTimeSegment = currentTimeSegment;

            // argh I don't have any time info for this!!!!
            if (userActivity.IsActiveSegment)
            {
                var primaryActivity = userActivity.PrimaryActivityInSegment;
                newTimeSegment.PrimaryUserActivity = UserActivity.ActiveSample( userActivity.SecondsInPrimarySegment, 
                                                                                primaryActivity.Name, 
                                                                                primaryActivity.WindowTitle);
            }
            else
            {
                newTimeSegment.PrimaryUserActivity = UserActivity.InactiveSample(userActivity.SecondsInPrimarySegment);
            }

            newTimeSegment.PrimaryUserHint = new UserHint();

            // add all process samples
            foreach (var sample in userActivity.Samples)
            {
                var activity = new UserActivityInTimeSegment(currentTimeSegment);
                activity.Activity = new UserActivity(sample.WasActive, sample.Seconds, sample.Details.Name, sample.Details.WindowTitle);

                newTimeSegment.AllUserActitivies.Add(activity);
            }
            
            return newTimeSegment;
        }
        
        private IDataContext dataContext;
        private ITimeSegmentProvider timeSegmentProvider;
    }
}
