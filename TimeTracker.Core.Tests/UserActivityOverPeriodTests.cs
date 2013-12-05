using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.Core.DataCollection;
using FluentAssertions;

namespace TimeTracker.Core.Tests
{
    [TestClass]
    public class UserActivityOverPeriodTests
    {
        private class TestUserActivity : IEqualish<TestUserActivity>
        {
            public bool IsEqualishTo(TestUserActivity other)
            {
                return true;
            }
        }

        [TestMethod]
        public void HasCorrectInitialValues()
        {
            var userActivity = new UserActivityOverPeriod<TestUserActivity>();

            userActivity.IsActiveSegment.Should().BeFalse("because no samples have been added yet");
            userActivity.TotalSecondsInSegment.Should().Be(0, "because no samples have been added yet");
            userActivity.PrimaryActivityInSegment.Should().BeNull("because no samples have been added yet");
        }
    }
}
