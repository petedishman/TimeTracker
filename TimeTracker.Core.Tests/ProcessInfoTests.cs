using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.Core.DataCollection.UserActivity;
using FluentAssertions;

namespace TimeTracker.Core.Tests
{
    [TestClass]
    public class ProcessInfoTests
    {
        [TestMethod]
        public void ProcessInfo_ConstructorShouldSetProperties()
        {
            var name = "process.exe";
            var title = "window title";
            var info = new ProcessInfo(name, title);

            info.Name.Should().BeEquivalentTo(name);
            info.WindowTitle.Should().BeEquivalentTo(title);
        }

        [TestMethod]
        public void ProcessInfo_TestEqualityWorks()
        {
            var info1 = new ProcessInfo("process.exe", "window title");
            var info2 = new ProcessInfo("process.exe", "window title");
            var info3 = new ProcessInfo("process.exe", "different window title");
            var info4 = new ProcessInfo("proc.exe", "window title");

            info1.Equals(info2).Should().BeTrue("because the two objects have the same process/window title");
            info1.Equals(info3).Should().BeFalse("because the window titles are different");
            info1.Equals(info4).Should().BeFalse("because the process titles are different");
        }

        [TestMethod]
        public void ProcessInfo_TestEqualishWorksWithUnsavedDocumentMarkers()
        {
            var processName = "process.exe";
            var info1 = new ProcessInfo(processName, "TestDocument.doc *");
            var info2 = new ProcessInfo(processName, "TestDocument.doc");

            info1.IsEqualishTo(info2).Should().BeTrue("because the process name is the same and the window title is basically the same!");
        }

        [TestMethod]
        public void ProcessInfo_TestEqualishWorksAlwaysReturnsFalseForDifferentProcesses()
        {
            var info1 = new ProcessInfo("process.exe", "TestDocument.doc *");
            var info2 = new ProcessInfo("process2.exe", "TestDocument.doc");

            info1.IsEqualishTo(info2).Should().BeFalse("because they're different processes");
        }

        [TestMethod]
        public void ProcessInfo_TestEqualsHandlesNull()
        {
            var info = new ProcessInfo("proc.exe", "titl");

            info.Equals((ProcessInfo)null).Should().BeFalse("because comparing against null should return false");
        }
    }
}
