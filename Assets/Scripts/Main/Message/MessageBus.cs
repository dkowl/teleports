using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MessageBus : IMessageBus {

    MessageBus instance;

    IDictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();

    public void Subscribe(object subscriber)
    {
        foreach(Type type in GetHandledMessageTypes(subscriber))
        {
            if (!subscribers.ContainsKey(type))
                subscribers.Add(type, new List<object>());

            subscribers[type].Add(subscriber);
        }
    }

    public void Unsubscribe(object subscriber)
    {
        foreach(Type type in GetHandledMessageTypes(subscriber))
        {
            if (!subscribers.ContainsKey(type))
            {
                Debug.LogWarning("Unsubscribing from a message you are not subscribed to");
                continue;
            }
            subscribers[type].Remove(subscriber);
        }
    }

    public void Publish<T>(T message) where T : IMessage
    {
        if(!subscribers.ContainsKey(typeof(T)) || subscribers[typeof(T)].Count == 0)
        {
            //Debug.LogWarning("Message " + typeof(T).FullName + "has no subscribers");
            return;
        }

        foreach(var subscriber in subscribers[typeof(T)])
        {
            ((IMessageHandler<T>)subscriber).Handle(message);
        }
    }

    List<Type> GetHandledMessageTypes(object subscriber)
    {
        return subscriber.GetType()
            .GetInterfaces()
            .Where(handler => handler.IsGenericType && handler.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
            .Select(handler => handler.GetGenericArguments().Single())
            .ToList();
    }
}
