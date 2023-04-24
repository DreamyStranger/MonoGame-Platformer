using System;
using System.Collections.Generic;

namespace ECS_Framework
{
    /// <summary>
    /// Represents a message that can be published and subscribed to.
    /// </summary>
    public interface IMessage { }

    /// <summary>
    /// Provides a message bus implementation for decoupled communication
    /// between components using a publish-subscribe pattern.
    /// </summary>
    public class MessageBus
    {
        // A dictionary that holds the subscription data.
        // The keys are message types, and the values are lists of actions to be invoked when a message is published.
        private static Dictionary<Type, List<Action<IMessage>>> subscribers = new Dictionary<Type, List<Action<IMessage>>>();

        /// <summary>
        /// Subscribes an action to a specific message type.
        /// </summary>
        /// <typeparam name="T">The type of message to subscribe to. Must implement IMessage.</typeparam>
        /// <param name="action">The action to be executed when a message of the specified type is published.</param>
        public static void Subscribe<T>(Action<T> action) where T : IMessage
        {
            if (!subscribers.ContainsKey(typeof(T)))
            {
                subscribers.Add(typeof(T), new List<Action<IMessage>>());
            }

            subscribers[typeof(T)].Add(message => action((T)message));
        }

        /// <summary>
        /// Unsubscribes an action from a specific message type.
        /// </summary>
        /// <typeparam name="T">The type of message to unsubscribe from. Must implement IMessage.</typeparam>
        /// <param name="action">The action to be removed from the list of subscribers for the specified message type.</param>
        public static void Unsubscribe<T>(Action<T> action) where T : IMessage
        {
            Type messageType = typeof(T);

            if (subscribers.ContainsKey(messageType))
            {
                subscribers[messageType].RemoveAll(subscriber => subscriber.Target == action.Target && subscriber.Method == action.Method);

                // Remove the key from the dictionary if there are no subscribers left for that message type.
                if (subscribers[messageType].Count == 0)
                {
                    subscribers.Remove(messageType);
                }
            }
        }

        /// <summary>
        /// Publishes a message to all subscribers of the message type.
        /// </summary>
        /// <param name="message">The message to be published.</param>
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
