using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.Schema.Entities
{
    public class TimeSlice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TimeSegment { get; set; }
        public bool WasUserActive { get; set; }
        public string PrimaryProcess { get; set; }
        public string PrimaryWindowTitle { get; set; }
        public int? PrimaryActivityDuration { get; set; }
        public string IssueKey { get; set; }
        public string IssueTitle { get; set; }
        public string Hint { get; set; }
        public bool Submitted { get; set; }
    }
}
