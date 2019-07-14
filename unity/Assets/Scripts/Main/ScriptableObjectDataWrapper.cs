using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class ScriptableObjectDataWrapper<T> : SerializedScriptableObject
{
    [OdinSerialize] protected T data;

    public T Data => data;
    public bool Empty => data == null;

#if UNITY_EDITOR
    protected void AddScriptableObjectWrappedData<T, TWrapper>(MappedList<T> mappedList)
        where T : IUniqueName
        where TWrapper : ScriptableObjectDataWrapper<T>
    {
        AssetEditor.AddScriptableObjectWrappedDataOfType<T, TWrapper>(this, mappedList);
    }
#endif
}
