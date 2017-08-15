using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameData : ScriptableObject {

    [SerializeField]
    [FormerlySerializedAs("races_")]
    private Race[] races;

    [SerializeField]
    private UnitAttributes unitAttributes;
}
