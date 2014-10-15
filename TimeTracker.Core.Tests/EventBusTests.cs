using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace TimeTracker.Core.Tests
{   
    [TestClass]
    public class EventBusTests
    {
        class EmptyTestMessage { }
        class EmptyTestMessage2 { }
        class ChildEmptyTestMessage : EmptyTestMessage { }

        [TestMethod]
        public void TestSingleReceiver()
        {
            var eventBus = new EventBus();
            var testMessage = new EmptyTestMessage();

            bool handlerCalled = false;

            eventBus.Register((EmptyTestMessage message) => {
                message.Should().BeSameAs(testMessage, "because we should have received the object we sent");
                handlerCalled = true;
            });

            eventBus.Send(testMessage);
            handlerCalled.Should().BeTrue("because otherwise the handler wasn't called");            
        }

        [TestMethod]
        public void TestMultipleReceivers()
        {
            var eventBus = new EventBus();
            var testMessage = new EmptyTestMessage();

            bool handlerCalled1 = false;
            bool handlerCalled2 = false;

            eventBus.Register((EmptyTestMessage message) => {
                message.Should().BeSameAs(testMessage, "because all receivers should have received the object we sent");
                handlerCalled1 = true;
            });

            eventBus.Register((EmptyTestMessage message) => {
                message.Should().BeSameAs(testMessage, "because all receivers should have received the object we sent");
                handlerCalled2 = true;
            });

            eventBus.Send(testMessage);

            handlerCalled1.Should().BeTrue("because otherwise the handler wasn't called");
            handlerCalled2.Should().BeTrue("because otherwise the handler wasn't called");
        }

        [TestMethod]
        public void TestCorrectMessageIsReceived()
        {
            var eventBus = new EventBus();
            var testMessage = new EmptyTestMessage();

            bool handlerCalled = false;

            eventBus.Register((EmptyTestMessage message) => {
                message.Should().BeSameAs(testMessage, "because we should have received the object we sent");
                handlerCalled = true;
            });

            eventBus.Register((EmptyTestMessage2 message) => {
                throw new Exception("Handler should not be called");
            });

            eventBus.Send(testMessage);
            handlerCalled.Should().BeTrue("because otherwise the correct handler wasn't called");
        }

        [TestMethod]
        public void TestHandlersForAChildClassShouldNotBeCalledWhenParentClassIsSent()
        {
            var eventBus = new EventBus();
            var testMessage = new EmptyTestMessage();

            eventBus.Register((ChildEmptyTestMessage message) => {
                throw new Exception("Handlers for a class should not be called when messages of a parent class are sent");
            });

            eventBus.Send(testMessage);
        }

        [TestMethod]
        public void TestHandlersForABaseClassShouldReceiveDerivedClassMessages()
        {
            var eventBus = new EventBus();
            var testMessage = new ChildEmptyTestMessage();

            bool handlerCalled = false;

            eventBus.Register((EmptyTestMessage message) => {
                message.Should().BeSameAs(testMessage, "because we should have received the object we sent");
                handlerCalled = true;
            });

            eventBus.Send(testMessage);

            handlerCalled.Should().BeTrue("because handlers registered for a base class should receive derived class messages");
        }

    }
}
