using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class InventoryEquipButton : LoadableBehaviour {

    InventoryMenu parentMenu;
    Button button;
    Text text;
    bool isInventorySlotSelected;

    protected override void LoadDataInternal()
    {
        if(parentMenu == null)
        {
            parentMenu = GetComponentInParent<InventoryMenu>();
        }
        Debug.Assert(parentMenu != null);

        if(button == null)
        {
            button = GetComponent<Button>();
        }
        Debug.Assert(button != null);

        if(text == null)
        {
            text = GetComponentInChildren<Text>();
        }
        Debug.Assert(text != null);

        isInventorySlotSelected = parentMenu.IsInventorySlotSelected;

        if (isInventorySlotSelected)
        {
            text.text = "EQUIP";
        }
        else
        {
            text.text = "UNEQUIP";
        }
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        InventoryMenu.UpdateUiEvent += LoadData;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        InventoryMenu.UpdateUiEvent -= LoadData;
    }

    public void OnClick()
    {
        if (isInventorySlotSelected)
        {
            OnEquip();
        }
        else
        {
            OnUnequip();
        }
    }

    public void DoNothing()
    {
        EquipmentData.CanEquipResult canEquipResult = parentMenu.InventoryData.EquipmentData.CanEquip(parentMenu.SelectedItem);
    }

    private void OnEquip()
    {

    }

    private void OnUnequip()
    {

    }
}
