using System;
namespace TimeTracker.Core.DataCollection
{
    public interface IUserActivityAggregator
    {
        event UserActivitySampleEventHandler UserActivitySampleReady;
    }
}
