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

    protected override void LoadDataInternal()
    {
        HeroData heroData = Main.GameState.CurrentHeroData;

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
                if (heroData == null)
                    titleString = newCharacterString;
                else
                    titleString = heroData.CharacterName;
                break;
        }
        titleText.text = titleString;
        if (heroData != null)
        {
            playerNameText.text = heroData.CharacterName;
            playerLevelText.text = "Level " + heroData.Level;
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
