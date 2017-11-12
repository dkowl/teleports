using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MappedListOfRaces : MappedList<Race>
{
    public List<Race> GetPlayableRaces()
    {
        var result = new List<Race>();

        foreach(Race race in list)
        {
            if (race.IsPlayable)
            {
                result.Add(race);
            }
        }

        return result;
    }
}
