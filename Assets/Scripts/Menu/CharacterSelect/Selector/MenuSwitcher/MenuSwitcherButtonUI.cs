using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwitcherButtonUI : SelectorButtonUI {

    public MenuController.MenuType menuType;

    public string buttonString;
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
}
