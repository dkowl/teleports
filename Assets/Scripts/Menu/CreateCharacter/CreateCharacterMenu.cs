using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using InputField = TMPro.TMP_InputField;
using System;

public class CreateCharacterMenu : MonoBehaviour {

    //Data
    List<Race> races;
    int raceId = 0;

    //UI elements
    [SerializeField] Text raceText;
    [SerializeField] Text raceDescriptionText;
    [SerializeField] InputField nameInputField;

    public event Action RaceIdChangedEvent;

    void Awake()
    {
        raceId = 0;
        OnRaceIdChanged();
    }

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

    public void CreateCharacter()
    {
        MainData.CurrentSaveData.CreateNewPlayer(Name, Race.UniqueName);
        Return();
    }
    
    public void Return()
    {
        MenuController.Instance.OpenMenu(MenuController.MenuType.ChooseCharacter);
    }

    public string Name
    {
        get { return nameInputField.text; }
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
        if(RaceIdChangedEvent != null)
            RaceIdChangedEvent();
    }
}
