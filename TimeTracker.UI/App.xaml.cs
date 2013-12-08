using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeTracker.Core;

namespace TimeTracker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            // so this is us starting up
            // we need to initialise our core objects and start tracking user/process activity

            // start talking to the database etc


            // need to get the mainwindow and call show on it
            // Our DI Container should get that for us.
            // maybe time to wire up ninject?

            kernel = new StandardKernel(new CoreDependencyModule(), new DependencyModule());
            timeTracker = kernel.Get<TimeTrackerAgent>();
        }

        IKernel kernel;
        TimeTrackerAgent timeTracker;
    }
}
