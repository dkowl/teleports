using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MappedList<T> where T : IUniqueName {

    [SerializeField]
    private List<T> list = new List<T>();
    
    private Dictionary<string, T> dict;

    public void MakeDict()
    {
        dict = new Dictionary<string, T>();

        foreach (T item in list)
        {
            dict.Add(item.UniqueName, item);
        }

        Debug.Log("Dictionary of type " + typeof(T).ToString() + " created from " + dict.Count.ToString() + " items");
    }

}
