using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMessageHandler<T> where T : IMessage {

    void Handle(T message);
}
