How does this work then?

For Process Tracking we can create an instance of an IActivityMonitor
We then need to periodically ask that for it's data

We then need to actually do something with that data, i.e. write it in to a database

The user can also specify what they're doing at any point, and that's either a random bit of
text or it's an Issue Key/Title

We want to write that in to the database as well.

So, what's our database structure then?

Need to store data for every day

I think the only way you can do it is to use TimePeriods, so from 00:00 to 00:15 we did this
And they should surely just be fixed?

In each time period we then have a few things to store

For processes we've got a list of Processes that you were using and how long for

For other things we've got the issue / string you had written down, but again there's multiple of those


UserActivity
Date
TimePeriod - Just a number, i.e. 0..96 for 15 minutes or could be a time
IssueKey
IssueTitle

ProcessActivity
Date
TimePeriod
ProcessTitle
Percentage

OtherActivity
Date
TimePeriod
IssueKey
IssueTitle


