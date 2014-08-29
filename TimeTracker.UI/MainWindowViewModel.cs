using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Services;
using TimeTracker.UI.ViewModels;

namespace TimeTracker.UI
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly IWindow window;

        private IDataContext dataContext;

        public MainWindowViewModel(IWindow window, IDataContext dataContext)
        {
            this.window = window;
            this.dataContext = dataContext;

            date = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            PropertyChanged += OnPropertyChanged;
            //activities = new List<TimeSegmentActivityViewModel>(GetDefaultList());
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentDate":
                    LoadDataForDate(CurrentDate);
                    break;
            }
        }

        private void LoadDataForDate(DateTime date)
        {
            var newActivityList = new List<TimeSegmentActivityViewModel>();//GetDefaultList());
            /*var timeSegments = dataContext.TimeSegments.FindWhere(x => x.StartOfTimeSegment.Date == date.Date);
            if (timeSegments != null)
            {
                // for each of these initialise our list
            }*/

            activities = newActivityList;
        }



        public DateTime CurrentDate 
        { 
            get 
            {
                return date;
            }
            set 
            { 
                if (date != value)
                {
                    date = value;
                    RaisePropertyChanged("CurrentDate");
                }
            }
        }

        public IEnumerable<TimeSegmentActivityViewModel> TimeSegmentActivities 
        { 
            get
            {
                return activities;
            }
            set
            {
                activities = value.ToList();
                RaisePropertyChanged("TimeSegmentActivities");
            }
        }

        private DateTime date;
        private List<TimeSegmentActivityViewModel> activities;
    }

    class DailyActivityViewModel
    {
        public DailyActivityViewModel(IDataContext dataContext)
        {
            this.dataContext = dataContext;
            activities = new List<TimeSegmentActivityViewModel>(GetEmptyDailyActivityList());
        }

        private IDataContext dataContext;

        private const int FiveMinuteSegmentsInADay = 288; // seconds in day = 86400, divided by 300 = 288
        private const int SecondsInFiveMinutes = 300;
        private static List<TimeSegmentActivityViewModel> EmptySingleDayTimeSegmentList = null;

        private static IReadOnlyList<TimeSegmentActivityViewModel> GetEmptyDailyActivityList()
        {
            if (EmptySingleDayTimeSegmentList == null)
            {
                EmptySingleDayTimeSegmentList = new List<TimeSegmentActivityViewModel>();
                var startOfDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
                for (int i = 0; i < FiveMinuteSegmentsInADay; i++)
                {
                    // date is irrelevant at this point
                    var segment = startOfDay.AddSeconds(i * SecondsInFiveMinutes);

                    // should we actually create a TimeSegment here?

                    EmptySingleDayTimeSegmentList.Add(new TimeSegmentActivityViewModel(segment));
                }
            }

            return EmptySingleDayTimeSegmentList.AsReadOnly();
        }

        /// <summary>
        /// Finds the matching (by time only) segment in our list and initialises it with timeSegment
        /// </summary>
        /// <param name="timeSegment"></param>
        public void InitialiseTimeSegment(TimeSegment timeSegment)
        {
            var existingActivity = activities.Where(x => x.StartOfTimeSegment.TimeOfDay == timeSegment.StartOfTimeSegment.TimeOfDay).FirstOrDefault();
            if (existingActivity != null)
            {
                //existingActivity.what?
            }
        }

        public IEnumerable<TimeSegmentActivityViewModel> Activities
        {
            get
            {
                return activities;
            }
        }
        private List<TimeSegmentActivityViewModel> activities;
    }

    class TimeSegmentActivityViewModel : ViewModelBase
    {
        public TimeSegmentActivityViewModel(DateTime startOfTimeSegment)
        {
            this._startOfTimeSegment = startOfTimeSegment;
            PrimaryProcessName = "";
            PrimaryProcessWindowTitle = "";
            PrimaryUserHint = "";
        }

        public void InitialiseFromTimeSegment(TimeSegment segment)
        {

        }

        private TimeSegment timeSegment = null;

        private DateTime _startOfTimeSegment;
        public DateTime StartOfTimeSegment
        {
            get
            {
                return _startOfTimeSegment;
            }
        }
        // the issue we're logging this timesegment against

        public string PrimaryProcessName { get;  set; }
        public string PrimaryProcessWindowTitle { get;  set; }

        public string PrimaryUserHint { get;  set; }
    }
}
