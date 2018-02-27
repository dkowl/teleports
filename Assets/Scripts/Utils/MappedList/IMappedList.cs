using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMappedList<T> where T : IUniqueName {

    bool ContainsName(string name);
    T GetValue(MappedListID id);
    List<T> GetValues(List<MappedListID> ids);


    IList<T> AllValues { get; }
    IList<string> AllNames { get; }
    T RandomValue { get; }
}
