using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorUI : MonoBehaviour {

    private static readonly string animName = "SelectorDown";

    new private Animation animation;
    private bool inSelectState = true;
    [Tooltip("Button text while selecting")]
    public string selectString;
    [Tooltip("Button text to start selection")]
    public string startString;
    public Text switchButtonText;
    public CharacterSelectMenu mainMenu;

    public void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void Hide()
    {
        if (inSelectState)
        {
            animation[animName].time = 0;
            animation[animName].speed = 1;
            switchButtonText.text = startString;
            mainMenu.SetState(CharacterSelectMenu.State.CurrentHero);
            inSelectState = false;
        }
        else
        {
            animation[animName].time = animation[animName].length;
            animation[animName].speed = -1;
            switchButtonText.text = selectString;
            mainMenu.SetState(CharacterSelectMenu.State.SelectHero);
            inSelectState = true;
        }

        animation.Play(animName);
    }
}
