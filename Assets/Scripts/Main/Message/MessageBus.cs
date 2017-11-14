using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageBus : IMessageBus {

    Dictionary<Type, List<object>> subscribers;

    public void Subscribe(object subscriber)
    {

    }

    public void Unsubscribe(object subscriber)
    {

    }

    public void Publish<T>(T message) where T : IMessage
    {

    }
}
