using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "race", menuName = "Custom/Race", order = 4)]
public class Race : UniqueScriptableObject {

    [FormerlySerializedAs("baseStats_")]
    [SerializeField]
    private UnitDataEditor baseStatsEditor;

    [SerializeField]
    private RaceGraphics graphics;

    private UnitData baseStats = null;
    
    public UnitData BaseStats
    {
        get {
            baseStats = new UnitData(baseStatsEditor);

            return baseStats;
        }
    }

    public RaceGraphics Graphics
    {
        get
        {
            return graphics;
        }
    }
}
