using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Core;
using TimeTracker.Core.DataCollection;
using TimeTracker.Core.DataCollection.UserActivity;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Infrastructure;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataContext dataContext = new SqlDataContext();

            var tse = dataContext.TimeSegments.FindAll().First();

            Console.WriteLine(tse);

            return;
            DateTime startOfTimeSegment = new DateTime(2013, 12, 1, 23, 0, 0);
            TimeSegment ts = new TimeSegment();
            ts.StartOfTimeSegment = startOfTimeSegment;
            ts.PrimaryUserActivity = new UserActivity() { ProcessName = "explorer.exe", WindowTitle = "c:\\", Seconds = 100, WasActive = true };
            ts.PrimaryUserHint = new UserHint() { HintText = "Just testing", Seconds = 20, WasActive = true };
            ts.AllUserActitivies = new List<UserActivityInTimeSegment>();
            ts.AllUserHints = new List<UserHintInTimeSegment>();

            var primaryActivity = new UserActivityInTimeSegment();
            primaryActivity.Activity = ts.PrimaryUserActivity;
            ts.AllUserActitivies.Add(primaryActivity);

            var primaryHint = new UserHintInTimeSegment();
            primaryHint.Hint = ts.PrimaryUserHint;
            ts.AllUserHints.Add(primaryHint);

            dataContext.TimeSegments.Add(ts);
            dataContext.Commit();

            return;

            var activeProcessHelper = new ActiveProcessHelper();
            var userActivityMonitor = new UserActivityMonitor();

            /*
            while (true)
            {
                var activeProcess = activeProcessHelper.GetActiveProcess();
                var timestamp = DateTime.Now.ToShortTimeString();
                Console.WriteLine("{0} {1} - {2}", timestamp, activeProcess.Name, activeProcess.WindowTitle);
                Console.WriteLine("Last active {0} seconds ago", userActivityMonitor.SecondsSinceLastUserActivity);
                Thread.Sleep(1000);
                
            }
            */

            // so in the actual TimeTracker process, what would we have
            // Every 5 minutes we'd want to be writing to the database
            // Don't need to be storing anything up for the whole day
            ProcessActivitySampler activitySampler = new ProcessActivitySampler(userActivityMonitor, activeProcessHelper);

            while (true)
            {
                Thread.Sleep(30 * 1000);

                UserActivityOverPeriod<ProcessInfo> timeSegment = activitySampler.StartNewTimeSegment();

                if (timeSegment.IsActiveSegment)
                {
                    var primaryProcess = timeSegment.PrimaryActivityInSegment;
                    Console.WriteLine("Primary activity: {0}", primaryProcess.WindowTitle);
                }
                else
                {
                    Console.WriteLine("This period was primarily inactive");
                }

                int secondsInSegment = timeSegment.TotalSecondsInSegment;
                foreach (UserActivity<ProcessInfo> sample in timeSegment.Samples)
                {
                    string sampleActivity;
                    int sampleDuration = (int)(((double)sample.Seconds / (double)secondsInSegment) * 100);

                    if (sample.WasActive)
                    {
                        sampleActivity = String.Format("{0}:{1}", sample.Details.Name, sample.Details.WindowTitle);
                    }
                    else
                    {
                        sampleActivity = "Inactive";
                    }

                    Console.WriteLine(" - {0} - {1}", sampleDuration, sampleActivity);
                }
            }
        }
    }
}
