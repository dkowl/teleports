using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gem", menuName = "Data/Gem")]
public class Gem : UniqueScriptableObject {

    [SerializeField]
    private string worldName;

    public string WorldName
    {
        get { return worldName; }
    }
}
