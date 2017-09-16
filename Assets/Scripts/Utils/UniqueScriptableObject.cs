using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueScriptableObject : ScriptableObject, IUniqueName {

    [SerializeField]
    private string uniqueName;

    public string UniqueName
    {
        get { return uniqueName; }
    }
}
