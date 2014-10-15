using Atlassian.Jira;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Core;
using TimeTracker.Core.DataCollection;
using TimeTracker.Core.DataCollection.UserActivity;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Infrastructure;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.ScratchPad
{
    class TimeTrackerAgent
    {
        public TimeTrackerAgent(DataLogger dataLogger)
        {
            this.dataLogger = dataLogger;
            
        }

        private DataLogger dataLogger;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new CoreDependencyModule());
            var timeTracker = kernel.Get<TimeTrackerAgent>();


            /*
            var jira = new Jira("http://jira/", "pete.dishman", "56Kilkea");

            var issues = from i in jira.Issues
                         where i.Assignee == "pete.dishman"
                            && i.Status == "Open"
                         orderby i.Key
                         select i;

            foreach (var i in issues)
            {
                Console.WriteLine(i.Summary);
                // you can add worklogs to an issue easily, which means we can log time directly in to jira
                // need to extend the api of the jira code though to allow us to specify the starttime of the worklog
                // the internal part of the api exposes it, so that should be trivial
                //i.AddWorklog()
            }
            */
        }
    }
}
