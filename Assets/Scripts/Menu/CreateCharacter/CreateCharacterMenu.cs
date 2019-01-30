using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using InputField = TMPro.TMP_InputField;
using System;

public class CreateCharacterMenu : MonoBehaviour {

    //Data
    private List<Race> races;
    private int raceId;

    //UI elements
    [SerializeField] private Text raceText;
    [SerializeField] private Text raceDescriptionText;
    [SerializeField] private InputField nameInputField;

    public event Action RaceIdChangedEvent;

    private void Awake()
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
        Main.GameState.CreateNewHero(Name, new RaceID(Race.UniqueName));
        Return();
    }
    
    public void Return()
    {
        MenuController.Instance.OpenMenu(MenuController.MenuIdSelectHero);
    }

    public string Name => nameInputField.text;

    public Race Race => Races[raceId];

    private List<Race> Races => races ?? (races = Main.StaticData.Game.GetPlayableRaces());

    private void OnRaceIdChanged()
    {
        raceText.text = Race.UniqueName;
        raceDescriptionText.text = Race.Description;
        RaceIdChangedEvent?.Invoke();
    }
}
