using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CharacterSelectMenu : LoadableBehaviour {

    public enum State
    {
        SelectHero,
        NewHero,
        CurrentHero
    }

    private State state = State.CurrentHero;

    public string selectCharacterString, newCharacterString;
    public Text titleText;
    public Text playerNameText;
    public Text playerLevelText;

    public delegate void MenuStateSwitched();
    public event MenuStateSwitched MenuStateSwitchedEvent;

    public override void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;

        string titleString = "";
        switch (state)
        {
            case State.SelectHero:
                titleString = selectCharacterString;
                break;
            case State.NewHero:
                titleString = newCharacterString;
                break;
            case State.CurrentHero:
                titleString = MainData.CurrentPlayerData.CharacterName;
                break;
        }
        titleText.text = titleString;
        playerNameText.text = MainData.CurrentPlayerData.CharacterName;
        playerLevelText.text = "Level " + MainData.CurrentPlayerData.Level.ToString();
    }

    public void SetState(State newState)
    {
        state = newState;
        LoadData();
        MenuStateSwitchedEvent();
    }

    public State GetState()
    {
        return state;
    }
}
