using System;
using System.Collections.Generic;

namespace ECS_Framework
{
    public interface IMessage { }
    public class MessageBus
    {
        private static Dictionary<Type, List<Action<IMessage>>> subscribers = new Dictionary<Type, List<Action<IMessage>>>();

        public static void Subscribe<T>(Action<T> action) where T : IMessage
        {
            if (!subscribers.ContainsKey(typeof(T)))
            {
                subscribers.Add(typeof(T), new List<Action<IMessage>>());
            }

            subscribers[typeof(T)].Add(message => action((T)message));
        }

        public static void Publish(IMessage message)
        {
            Type messageType = message.GetType();

            if (subscribers.ContainsKey(messageType))
            {
                foreach (var subscriber in subscribers[messageType])
                {
                    subscriber(message);
                }
            }
        }
    }
}
