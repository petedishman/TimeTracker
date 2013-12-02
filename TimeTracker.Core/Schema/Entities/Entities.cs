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
        [Key]
        public long Id { get; set; }
        public DateTime StartOfTimeSegment { get; set; }
    }

    public class UserActivityInTimeSegment : TimeSegmentDetails
    {
        public UserActivity Activity { get; set; }
    }

    public class UserHintInTimeSegment : TimeSegmentDetails
    {
        public UserHint Hint { get; set; }
    }
}
