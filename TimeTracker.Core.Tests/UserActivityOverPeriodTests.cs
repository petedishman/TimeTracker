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
            private string detail;

            public TestUserActivity(string detail)
            {
                this.detail = detail;
            }

            public bool IsEqualishTo(TestUserActivity other)
            {
                return this.detail.Equals(other.detail, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        private TestUserActivity testActivity1 = new TestUserActivity("Test1");
        private TestUserActivity testActivity2 = new TestUserActivity("test1");
        private TestUserActivity testActivity3 = new TestUserActivity("Test Activity");
        private TestUserActivity testActivity4 = new TestUserActivity("Another Activity");

        private UserActivityOverPeriod<TestUserActivity> userActivity;

        [TestInitialize]
        public void Setup()
        {
            userActivity = new UserActivityOverPeriod<TestUserActivity>();
        }

        [TestCleanup]
        public void Teardown()
        {
            userActivity = null;
        }

        [TestMethod]
        public void UserActivityOverPeriod_HasCorrectInitialValues()
        {
            userActivity.IsActiveSegment.Should().BeFalse("because no samples have been added yet");
            userActivity.TotalSecondsInSegment.Should().Be(0, "because no samples have been added yet");
            userActivity.PrimaryActivityInSegment.Should().BeNull("because no samples have been added yet");
        }

        [TestMethod]
        public void UserActivityOverPeriod_SumOfSecondsAddsUp()
        {
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();
            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity2);
            userActivity.AddActiveSample(testActivity3);

            userActivity.TotalSecondsInSegment.Should().Be(5, "because 5 samples have been added");
        }
        
        [TestMethod]
        public void UserActivityOverPeriod_AddNullSampleShouldThrowException()
        {
            Action addNullSample = () => userActivity.AddActiveSample(null);

            addNullSample.ShouldThrow<ArgumentNullException>().
                And.ParamName.Should().Be("details");
        }
        
        [TestMethod]
        public void UserActivityOverPeriod_MatchingActivityDetailsCountAsOneSample()
        {
            // These should count as one
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();

            // These should count as one
            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity2);

            userActivity.AddActiveSample(testActivity4);

            userActivity.Samples.Count.Should().Be(3, "because 3 different matching samples have been added");
        }

        [TestMethod]
        public void UserActivityOverPeriod_PeriodCountsAsInactiveWhenMajoritySamplesAreInactive()
        {
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();

            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity2);
            userActivity.AddActiveSample(testActivity3);
            userActivity.AddActiveSample(testActivity4);

            userActivity.IsActiveSegment.Should().BeFalse("because we added 4 inactive samples and 4 unique samples");
            userActivity.PrimaryActivityInSegment.Should().BeNull("because this isn't an active segment");
        }

        [TestMethod]
        public void UserActivityOverPeriod_SampleTotalsAddUpAndMatchCorrectly()
        {
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();
            userActivity.AddInactiveSample();

            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity1);
            userActivity.AddActiveSample(testActivity2);
            userActivity.AddActiveSample(testActivity2);

            userActivity.AddActiveSample(testActivity4);

            userActivity.TotalSecondsInSegment.Should().Be(8, "because 8 samples have been added");
            userActivity.PrimaryActivityInSegment.Should().Be(testActivity1, "because that's the primary activity!");
            foreach (var sample in userActivity.Samples)
            {
                if (sample.WasActive)
                {
                    if (sample.Details.IsEqualishTo(testActivity1))
                    {
                        sample.Seconds.Should().Be(4, "because we added 3 other samples equalish to testActivity1");
                    }
                    else if (sample.Details.IsEqualishTo(testActivity4))
                    {
                        sample.Seconds.Should().Be(1, "because we only added 1 sample like testActivity4");
                    }
                }
                else
                {
                    sample.Seconds.Should().Be(3, "because we added 3 inactive samples");
                }
            }
        }
    }
}
