using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection.UserActivity
{
    public class ProcessInfo : IEqualish<ProcessInfo>, IEquatable<ProcessInfo>
    {
        public ProcessInfo(string name, string windowTitle)
        {
            this.Name = name;
            this.WindowTitle = windowTitle;
        }

        public string Name { get; private set; }
        public string WindowTitle { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProcessInfo);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsEqualishTo(ProcessInfo other)
        {
            // we could do some clever matching on window titles
            // like ignoring * for unsaved files and stuff
            // for now though, we'll just do a straight equals check
            return Equals(other);
        }

        public bool Equals(ProcessInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return (other.Name == this.Name &&
                    other.WindowTitle == this.WindowTitle);
        }
    }


    /// <summary>
    /// Should I just fix to 5 minute periods of the day and not worry about it.
    /// Yes, do it.
    /// 
    /// So we can log things against 5 minute sections of the day.
    /// Should make it pull rather than push too.
    /// 
    /// So create a class that logs process activity internally.
    /// Every 5 minutes we can pull info from it for a certain time period?
    /// </summary>


    /*

    public class UserActivityPeriodEventArgs : EventArgs
    {
        public UserActivityPeriodEventArgs()
        {

        }
    }

    /// <summary>
    ///  used to track user activity over time
    ///  an instance of this will be instantiated at the start of the program and it will
    ///  be used to track activity
    ///  
    ///  client creates an instance of this and calls StartTracking()
    ///  It can then periodically ask this class for what's been used in that period
    /// </summary>
    public interface IActivityMonitor
    {
        void StartTracking();
        void RestartTracking();
        //void PauseTracking();

       // delegate void UserActivityPeriodEventHandler(object sender, UserActivityPeriodEventArgs eventArgs);

        UserActivityPeriod GetUserActivity();
    }

    public class UserActivityQuanta
    {
        ProcessInfo Process { get; set; }
        TimeSpan Duration { get; set; }
    }

    public class UserActivityPeriod
    {
        DateTime StartTime;
        DateTime EndTime;
        TimeSpan Duration;

       // UserActivityQuanta PrimaryActivity { get; }
        //IList<UserActivityQuanta> Activities { get; }
    }
    */
}
