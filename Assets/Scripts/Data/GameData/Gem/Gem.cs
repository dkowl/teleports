using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gem", menuName = "Data/Gem")]
public class Gem : ScriptableObject, IUniqueName {

    [SerializeField]
    private string uniqueName;

    [SerializeField]
    private string worldName;

    public string UniqueName
    {
        get { return uniqueName; }
    }

    public string WorldName
    {
        get { return worldName; }
    }
}
