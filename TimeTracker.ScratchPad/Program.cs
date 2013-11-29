using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Core;

namespace TimeTracker.ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
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

                ProcessActivityTimeSegment timeSegment = activitySampler.StartNewTimeSegment();

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
                foreach (ProcessActivitySample sample in timeSegment.Samples)
                {
                    string sampleActivity;
                    int sampleDuration = (int)(((double)sample.Seconds / (double)secondsInSegment) * 100);

                    if (sample.WasActive)
                    {
                        sampleActivity = String.Format("{0}:{1}", sample.ProcessInfo.Name, sample.ProcessInfo.WindowTitle);
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
