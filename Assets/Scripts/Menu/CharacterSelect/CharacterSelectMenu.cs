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

    private State state = State.SelectHero;

    public string selectCharacterString, newCharacterString;
    public Text titleText;
    public Text playerNameText;
    public Text playerLevelText;

    public delegate void MenuStateSwitched();
    public event MenuStateSwitched MenuStateSwitchedEvent;

    public override void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;
        IPlayerData playerData = MainData.CurrentPlayerData;

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
                if (playerData == null)
                    titleString = newCharacterString;
                else
                    titleString = MainData.CurrentPlayerData.CharacterName;
                break;
        }
        titleText.text = titleString;
        if (playerData != null)
        {
            playerNameText.text = MainData.CurrentPlayerData.CharacterName;
            playerLevelText.text = "Level " + MainData.CurrentPlayerData.Level.ToString();
        }
        else
        {
            playerNameText.text = "Empty";
            playerLevelText.text = "";
        }
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
