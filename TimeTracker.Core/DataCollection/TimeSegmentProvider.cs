using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection
{
    class TimeSegmentProvider : TimeTracker.Core.DataCollection.ITimeSegmentProvider
    {
        private const int SecondsInFiveMinutes = 5 * 60;

        // what ever the current time is, we'll return the 
        // time of the current 5 minute segment of the day
        // so if it's 15:47:02, we'll return 15:45:00
        public DateTime CurrentTimeSegment
        {
            get
            {
                var currentTime = DateTime.Now;
                int secondsToday =  (currentTime.Hour * 60 * 60) +
                                    (currentTime.Minute * 60) +
                                     currentTime.Second;

                int fiveMinuteSegmentsSoFarToday = secondsToday / SecondsInFiveMinutes;

                DateTime currentSegment = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0);
                
                return currentSegment.AddSeconds(fiveMinuteSegmentsSoFarToday * SecondsInFiveMinutes);;
            }
        }
    }
}
