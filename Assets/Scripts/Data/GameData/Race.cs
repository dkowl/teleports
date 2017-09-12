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
    private UnitDataEditor baseStatsEditor;

    private UnitData baseStats = null;

    public string Name
    {
        get { return name; }
    }
    
    public UnitData BaseStats
    {
        get {
            if(baseStats == null || !baseStats.IsInitialized)
            {
                baseStats = new UnitData(baseStatsEditor);
            }

            return baseStats;
        }
    }
}
