using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect_MiddleButton : LoadableBehaviour {

    Button button;
    Text text;

	void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();

        button.onClick.AddListener(OnClick);
    }

    protected override void LoadDataInternal()
    {
        if (Main.GameState.CurrentHeroData == null)
        {
            text.text = "New Hero";
        }
        else {
            text.text = "Select";
        }
    }

    public void OnClick()
    {
        if (Main.GameState.CurrentHeroData == null)
        {
            MenuController.Instance.OpenMenu(MenuController.MenuIdCreateHero);
        }
        else
        {
            MenuController.Instance.OpenMenu(MenuController.MenuIdHome);
        }
    }
}
