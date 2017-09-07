using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "race", menuName = "Custom/Race", order = 4)]
public class Race : ScriptableObject {

    [FormerlySerializedAs("name_")]
    [SerializeField]
    private string raceName;

    [FormerlySerializedAs("baseStats_")]
    [SerializeField]
    private UnitData baseStats;
}
