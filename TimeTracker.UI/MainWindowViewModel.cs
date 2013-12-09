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
        }

        private DateTime date;
        private readonly ObservableCollection<TimeSegmentActivityViewModel> activities;
    }

    public class TimeSegmentActivityViewModel
    {
        public TimeSegmentActivityViewModel()
        {

        }

        public DateTime StartOfTimeSegment { get; private set; }
        // the issue we're logging this timesegment against

        public string PrimaryProcessName { get; private set; }
        public string PrimaryProcessWindowTitle { get; private set; }

        public string PrimaryUserHint { get; private set; }
    }
}
