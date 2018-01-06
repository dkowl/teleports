using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEquipButton : LoadableBehaviour {

    InventoryMenu parentMenu;

    protected override void LoadDataInternal()
    {
        if(parentMenu == null)
        {
            parentMenu = GetComponentInParent<InventoryMenu>();
        }
        Debug.Assert(parentMenu != null);

        bool isEquip = parentMenu.IsInventorySlotSelected;

    }

    public void OnClick()
    {
        var choices = new List<ButtonChoice>();
        choices.Add(new ButtonChoice("1", DoNothing));
        choices.Add(new ButtonChoice("2", DoNothing));
        DialogWindowSpawner.Spawn("Test", choices);
    }

    public void DoNothing()
    {

    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
    }

}
