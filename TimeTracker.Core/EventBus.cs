using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    public class EventBus
    {
        Dictionary<Type, List<object>> messageHandlers = new Dictionary<Type, List<object>>();

        public EventBus()
        {

        }

        public void Send<T>(T message)
        {
            // look through our list of types and then call all the handlers
            // if there is a handler for a parent class of T then we should include that too

            Type messageType = typeof(T);
            foreach(var handlers in messageHandlers.Where(kvp => kvp.Key.IsAssignableFrom(messageType)))
            {
                //var handlers = messageHandlers[messageType];
                foreach (var handler in handlers.Value)
                {
                    var typedHandler = handler as Action<T>;
                    typedHandler(message);
                }
            }
        }

        public void Register<T>(Action<T> onMessage)
        {
            Type messageType = typeof(T);

            if (!messageHandlers.ContainsKey(messageType))
            {
                messageHandlers[messageType] = new List<object>();
            }

            messageHandlers[messageType].Add(onMessage);
        }
    }
}
