using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class MappedList<T> where T : IUniqueName {

    [SerializeField]
    protected List<T> list = new List<T>();
    
    protected Dictionary<string, T> dict;

    public void MakeDict()
    {
        dict = new Dictionary<string, T>();

        foreach (T item in list)
        {
            dict.Add(item.UniqueName, item);
        }

        //Debug.Log("Dictionary of type " + typeof(T).ToString() + " created from " + dict.Count.ToString() + " items");
    }

    public T TryGetValue(string name)
    {
        if(dict == null)
        {
            MakeDict();
        }

        T result;
        if(name != null && dict.TryGetValue(name, out result))
        {
            return result;
        }
        else
        {
            return default(T);
        }
    }

    public IList<T> AllValues
    {
        get { return list.AsReadOnly(); }
    }

}
