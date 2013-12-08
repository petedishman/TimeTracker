using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UI
{
    class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TimeTrackerAgent>().ToSelf();
        }
    }
}
