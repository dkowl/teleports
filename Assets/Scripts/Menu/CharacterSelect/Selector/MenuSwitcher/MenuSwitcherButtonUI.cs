using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class MenuSwitcherButtonUI : SelectorButtonUI {

    private MenuController.MenuType menuType;
    private string buttonString;

    public Text buttonText;

    public override void LoadDataInternal()
    {
        base.LoadDataInternal();

        buttonText.text = buttonString;
    }

    protected override bool IsActive()
    {
        return MenuController.Instance.IsActive(menuType);
    }

    protected override void OnActivate()
    {
        MenuController.Instance.OpenMenu(menuType);
    }

    protected override void OnDeactivate()
    {
        MenuController.Instance.CloseMenu(menuType);
    }

    public MenuController.MenuType MenuType
    {
        get { return menuType; }
        set { menuType = value; }
    }

    public string ButtonString
    {
        get { return buttonString; }
        set { buttonString = value; }
    }
}
