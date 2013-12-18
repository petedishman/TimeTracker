using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UI
{
    public class MainWindowViewModel
    {
        private readonly IWindow window;

        public MainWindowViewModel(IWindow window)
        {
            this.window = window;

            CurrentDate = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            var activities = new List<TimeSegmentActivityViewModel>();
            activities.Add(new TimeSegmentActivityViewModel() { StartOfTimeSegment = new DateTime(2013, 12, 18, 22, 0, 0), PrimaryProcessName = "procexp.exe", PrimaryProcessWindowTitle = "Stuff", PrimaryUserHint = ""});
            TimeSegmentActivities = activities;
        }

        public DateTime CurrentDate { get; set; }

        public IEnumerable<TimeSegmentActivityViewModel> TimeSegmentActivities { get; set; }

        private DateTime date;
        private readonly ObservableCollection<TimeSegmentActivityViewModel> activities;
    }

    public class TimeSegmentActivityViewModel
    {
        public TimeSegmentActivityViewModel()
        {

        }

        public DateTime StartOfTimeSegment { get;  set; }
        // the issue we're logging this timesegment against

        public string PrimaryProcessName { get;  set; }
        public string PrimaryProcessWindowTitle { get;  set; }

        public string PrimaryUserHint { get;  set; }
    }
}
