using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class ScriptableObjectDataWrapper<T> : SerializedScriptableObject
{
    [SerializeField] protected T data;

    public T Data => data;
    public bool Empty => data == null;
}
