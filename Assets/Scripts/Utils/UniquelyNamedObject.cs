using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UniquelyNamedObject : IUniqueName
{
    [SerializeField] private string uniqueName;

    public string UniqueName
    {
        get { return uniqueName; }
        protected set { uniqueName = value; }
    }
}
