using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using TimeTracker.Core.Schema.Services;
using TimeTracker.Core.DataCollection;
using TimeTracker.Core.Schema.Infrastructure;
using TimeTracker.Core.DataCollection.UserActivity;

namespace TimeTracker.Core
{
    public class CoreDependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataContext>().To<SqlDataContext>();
            Bind<ITimeSegmentProvider>().To<TimeSegmentProvider>();
            Bind<IUserActivityAggregator>().To<UserActivityAggregator>();
            Bind<IUserActivityMonitor>().To<UserActivityMonitor>();
            Bind<IActiveProcessHelper>().To<ActiveProcessHelper>();

            Bind<DataLogger>().ToSelf();
        }
    }
}
