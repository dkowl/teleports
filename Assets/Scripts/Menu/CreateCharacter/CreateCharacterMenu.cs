using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using System;

public class CreateCharacterMenu : MonoBehaviour {

    //Data
    List<Race> races;
    int raceId = 0;

    //UI elements
    [SerializeField] Text raceText;
    [SerializeField] Text raceDescriptionText;

    public event Action RaceIdChangedEvent;

    public void IncrementRaceId()
    {
        raceId++;
        if (raceId >= Races.Count)
            raceId = 0;
        OnRaceIdChanged();
    }

    public void DecrementRaceId()
    {
        raceId--;
        if (raceId < 0)
            raceId = Races.Count - 1;
        OnRaceIdChanged();
    }

    public Race Race
    {
        get { return Races[raceId]; }
    }

    List<Race> Races{
        get
        {
            if(races == null)
            {
                races = MainData.CurrentGameData.GetPlayableRaces();
            }
            return races;
        }
    }

    void OnRaceIdChanged()
    {
        raceText.text = Race.UniqueName;
        raceDescriptionText.text = Race.Description;
        RaceIdChangedEvent();
    }
}
