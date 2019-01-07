using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class MenuSwitcherButtonUI : SelectorButtonUI {

    [SerializeField] private MenuID menuId;
    [SerializeField] private string buttonString;

    public Text buttonText;

    protected override void LoadDataInternal()
    {
        base.LoadDataInternal();

        buttonText.text = buttonString;
    }

    protected override bool IsActive()
    {
        return MenuController.Instance.IsActive(menuId);
    }

    protected override void OnActivate()
    {
        MenuController.Instance.OpenMenu(menuId);
    }

    protected override void OnDeactivate()
    {
        MenuController.Instance.CloseMenu(menuId);
    }

    public MenuID MenuId
    {
        get { return menuId; }
        set { menuId = value; }
    }

    public string ButtonString
    {
        get { return buttonString; }
        set { buttonString = value; }
    }
}
