
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    class IssueTracker
    {
    }

    // code for interacting with an Issue Tracker, i.e. Jira!
    // we want to be able to retrieve, and cache (to disk) the user's main
    // issues so that they're available after restarts / no connectivity
    // when connected we want to be able to search all issues / maybe limited by project

    public interface IIssueTrackerConfiguration
    {
        string Hostname { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Passowrd { get; set; }
    }

    public interface IIssue
    {
        string Key { get; }
        string Title { get; }
    }

    public interface IIssueTracker
    {
        IQueryable<IIssue> UserIssues { get; }
    }

    // what do we need
    // we want a list of all issues owned by the user
    // we also need a way of searching for issues that the user doesn't own
    // I don't really know how that'll work though?

}
