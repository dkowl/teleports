using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterMenu : MonoBehaviour {

    List<Race> races;

    void Awake()
    {
        races = MainData.CurrentGameData.GetPlayableRaces();
        foreach(Race race in races)
        {
            print(race.UniqueName);
        }
    }
}
