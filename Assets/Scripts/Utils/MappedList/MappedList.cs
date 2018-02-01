using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class MappedList<T> : IMappedList<T> where T : IUniqueName {

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

    public bool ContainsName(string name)
    {
        return dict.ContainsKey(name);
    }

    public IList<T> AllValues
    {
        get { return list.AsReadOnly(); }
    }

    public IList<string> AllNames
    {
        get
        {
            var names = new List<string>();
            names.Add("");
            foreach(var element in list)
            {
                names.Add(element.UniqueName);
            }
            return names;
        }
    }

    public T RandomValue
    {
        get
        {
            int id = Random.Range(0, list.Count);
            return list[id];
        }
    }

}
