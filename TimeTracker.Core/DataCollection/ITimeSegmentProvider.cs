using System;
namespace TimeTracker.Core.DataCollection
{
    public interface ITimeSegmentProvider
    {
        DateTime CurrentTimeSegment { get; }
    }
}
