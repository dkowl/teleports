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

    override protected void LoadDataInternal()
    {
        if (MainData.CurrentPlayerData == null)
        {
            text.text = "New Hero";
        }
        else {
            text.text = "Select";
        }
    }

    public void OnClick()
    {
        if (MainData.CurrentPlayerData == null)
        {
            MenuController.Instance.OpenMenu(MenuController.MenuType.CreateCharacter);
        }
        else
        {
            MenuController.Instance.OpenMenu(MenuController.MenuType.Home);
        }
    }
}
