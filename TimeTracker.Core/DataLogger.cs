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


            // Testing
            // Let's just make sure there's a load of data in our db for today
            // maybe?
        }

        void OnUserActivitySampleReady(object sender, UserActivitySampleEventArgs e)
        {
            // take the stats we just got and stick them in the database
            // for whatever the current time segment is

            // This is going to need some exception handling
            TimeSlice timeSlice = GetTimeSliceFromUserActivitySample(e.ProcessActivity);


            // TODO:
            // This will fail (won't it?) when there's already data recorded for this time segment
            // which should be possible to get when starting up multiple times? maybe? possibly not?
            // because we'd be triggered in 5 minutes time, so that should be a different segment?
            this.dataContext.TimeSlices.Add(timeSlice);
            this.dataContext.Commit();

            // TODO
            // fire an event off saying we've just saved some new data to the database 
            // sending out the TimeSegment along with it.
            // the UI can listen for that.
        }

        TimeSlice GetTimeSliceFromUserActivitySample(UserActivityOverPeriod<ProcessInfo> userActivity)
        {
            var newTimeSlice = new TimeSlice();
            var currentTimeSegment = this.timeSegmentProvider.CurrentTimeSegment;

            newTimeSlice.Date = this.timeSegmentProvider.CurrentTimeSegment;

            if (userActivity.IsActiveSegment)
            {
                newTimeSlice.PrimaryProcess = userActivity.PrimaryActivityInSegment.Name;
                newTimeSlice.PrimaryWindowTitle = userActivity.PrimaryActivityInSegment.WindowTitle;
                newTimeSlice.PrimaryActivityDuration = userActivity.SecondsInPrimarySegment;
            }
            else
            {
                newTimeSlice.PrimaryActivityDuration = null;
                newTimeSlice.PrimaryProcess = null;
                newTimeSlice.PrimaryWindowTitle = null;
            }
 
            return newTimeSlice;
        }
        
        private IDataContext dataContext;
        private ITimeSegmentProvider timeSegmentProvider;
    }
}
