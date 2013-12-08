using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.Schema.Entities
{
    /// <summary>
    /// For every 5 minute period in a day we need to store:
    /// 
    /// The primary activity that a user was doing process name / window title / seconds in use
    /// Plus all the other activities they were doing
    /// 
    /// The primar user hint that a user had selected, plus seconds in use
    /// a user hint is either an issue key or a random bit of text
    /// plus all the other user hints they had in that period
    /// 
    /// 
    /// </summary>

    public class TimeSegment
    {
        public TimeSegment()
        {
            // these two initialisations generate code analysis warnings due to the lists both being virtual
            AllUserActitivies = new List<UserActivityInTimeSegment>();
            AllUserHints = new List<UserHintInTimeSegment>();
        }

        [Key]
        public DateTime StartOfTimeSegment { get; set; }
        public UserActivity PrimaryUserActivity { get; set; }
        public UserHint PrimaryUserHint { get; set; }

        public virtual List<UserActivityInTimeSegment> AllUserActitivies { get; set; }
        public virtual List<UserHintInTimeSegment> AllUserHints { get; set; }
    }

    /// <summary>
    /// EF Complex types can't use inheritance, so we need to keep WasActive and Seconds
    /// in this class rather than moving out to a base class
    /// </summary>
    public class UserActivity
    {
        public UserActivity(bool wasActive, int seconds, string processName, string windowTitle)
        {
            this.WasActive = wasActive;
            this.Seconds = seconds;
            this.ProcessName = processName;
            this.WindowTitle = windowTitle;
        }

        public static UserActivity InactiveSample(int seconds)
        {
            return new UserActivity(false, seconds, null, null);
        }

        public static UserActivity ActiveSample(int seconds, string processName, string windowTitle)
        {
            return new UserActivity(true, seconds, processName, windowTitle);
        }

        public bool WasActive { get; set; }
        public int Seconds { get; set; }
        public string ProcessName { get; set; }
        public string WindowTitle { get; set; }
    }

    public class UserHint
    {
        public bool WasActive { get; set; }
        public int Seconds { get; set; }
        public string IssueKey { get; set; }
        public string IssueTitle { get; set; }
        public string HintText { get; set; }
    }

    public class TimeSegmentDetails
    {
        public TimeSegmentDetails(DateTime startOfTimeSegment)
        {
            this.StartOfTimeSegment = startOfTimeSegment;
        }

        [Key]
        public long Id { get; set; }
        public DateTime StartOfTimeSegment { get; set; }
    }

    public class UserActivityInTimeSegment : TimeSegmentDetails
    {
        public UserActivityInTimeSegment(DateTime startOfTimeSegment) : 
            base(startOfTimeSegment)
        {
            
        }

        public UserActivity Activity { get; set; }
    }

    public class UserHintInTimeSegment : TimeSegmentDetails
    {
        public UserHintInTimeSegment(DateTime startOfTimeSegment) : 
            base(startOfTimeSegment)
        {
            
        }
        
        public UserHint Hint { get; set; }
    }
}
