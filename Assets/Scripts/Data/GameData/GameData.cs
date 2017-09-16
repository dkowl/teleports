using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "gameData", menuName = "Data/Game")]
public class GameData : ScriptableObject {

    [SerializeField]
    [FormerlySerializedAs("races_")]
    private Race[] races;

    [SerializeField] private MappedListOfGems gems;
    [SerializeField] private MappedListOfWorlds worlds;

    public void OnEnable()
    {
        gems.MakeDict();
        worlds.MakeDict();
    }
    
    public Race GetRace(string raceName)
    {
        foreach(Race race in races)
        {
            if(race.Name == raceName)
            {
                return race;
            }
        }

        Debug.Log("Race " + raceName + " not found.");
        return null;
    }
}
