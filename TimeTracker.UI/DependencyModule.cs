using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeTracker.UI
{
    class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TimeTrackerAgent>().ToSelf();
            Bind<IMainWindowViewModelFactory>().To<MainWindowViewModelFactory>();

            Bind<Window>().To<MainWindow>();
            Bind<IWindow>().To<MainWindowAdapter>().InTransientScope();
        }
    }
}
