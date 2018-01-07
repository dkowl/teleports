using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Teleports.Utils;

public class InventoryEquipButton : LoadableBehaviour {

    InventoryMenu parentMenu;
    Button button;
    List<Image> images;
    Text text;
    bool isInitialized = false;
    State state;
    EquipmentData.CanEquipResult canEquipResult;
    List<ButtonChoice> okChoices = new List<ButtonChoice>();

    protected override void LoadDataInternal()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        state = GetCurrentState();
        UpdateView();
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
        switch (state)
        {
            case State.Equip_Yes:
                parentMenu.EquipSelected();
                break;
            case State.Equip_PrimaryConflict:
                parentMenu.EquipSelected();
                break;
            case State.Equip_Impossible:
                DialogWindowSpawner.Spawn("Impossible", okChoices);
                break;
            case State.Equip_RequirementsNotMet:
                DialogWindowSpawner.Spawn("Requirements not met", okChoices);
                break;
            case State.Unequip_Yes:
                parentMenu.UnequipSelected();
                break;
            case State.Unequip_InventoryFull:
                DialogWindowSpawner.Spawn("Inventory full", okChoices);
                break;

        }
    }

    public void DoNothing()
    {
    }

    private void Initialize()
    {
        if (parentMenu == null)
        {
            parentMenu = GetComponentInParent<InventoryMenu>();
        }
        Debug.Assert(parentMenu != null);

        if (button == null)
        {
            button = GetComponent<Button>();
        }
        Debug.Assert(button != null);

        if(images == null)
        {
            images = new List<Image>();
            foreach (var image in GetComponentsInChildren<Image>())
            {
                images.Add(image);
            }
        }

        if (text == null)
        {
            text = GetComponentInChildren<Text>();
        }
        Debug.Assert(text != null);

        okChoices.Add(new ButtonChoice("OK", DoNothing));

        isInitialized = true;
    }

    private State GetCurrentState()
    {
        ItemData selectedItem = parentMenu.SelectedItem;

        if (selectedItem == null)
        {
            return State.NoItem;
        }

        if (parentMenu.IsInventorySlotSelected)
        {
            if (EquipRequirementsMet())
            {
                canEquipResult = parentMenu.InventoryData.EquipmentData.CanEquip(parentMenu.SelectedItem);
                switch (canEquipResult.Status)
                {
                    case EquipmentData.CanEquipStatus.No_Impossible:
                        return State.Equip_Impossible;
                    case EquipmentData.CanEquipStatus.No_PrimaryConflict:
                        return State.Equip_PrimaryConflict;
                    case EquipmentData.CanEquipStatus.No_SecondaryConflict:
                        return State.Equip_SecondaryConflict;
                    case EquipmentData.CanEquipStatus.Yes:
                        return State.Equip_Yes;
                    default:
                        return State.Equip_Impossible;
                }
            }
            else
            {
                return State.Equip_RequirementsNotMet;
            }
        }
        else
        {
            InventoryData.CanUnequipStatus canUnequipStatus = parentMenu.InventoryData.CanUnequip(parentMenu.SelectedSlotId.equipmentSlotType);
            switch (canUnequipStatus)
            {
                case InventoryData.CanUnequipStatus.Yes:
                    return State.Unequip_Yes;
                case InventoryData.CanUnequipStatus.No_InventoryFull:
                    return State.Unequip_InventoryFull;
                default:
                    return State.NoItem;
            }
        }
    }

    private void UpdateView()
    {
        if(state == State.NoItem)
        {
            gameObject.MakeInvisible();
        }
        else
        {
            gameObject.MakeVisible();
        }

        if (IsEquipState(state))
        {
            text.text = "EQUIP";
        }
        else
        {
            text.text = "UNEQUIP";
        }

        SetDisabled(IsDisabledState(state));
    }

    private void SetDisabled(bool disabled = true)
    {
        float targetAlpha;
        if (disabled)
        {
            targetAlpha = 0.25f;
        }
        else
        {
            targetAlpha = 1;
        }
        foreach (var image in images)
        {
            Color color = image.color;
            color.a = targetAlpha;
            image.color = color;
        }

        TextStyler textStyler = text.GetComponent<TextStyler>();
        if(textStyler != null)
        {
            textStyler.AlphaMultiplier = targetAlpha;
        }
    }

    private bool EquipRequirementsMet()
    {
        bool result = false;
        ItemData selectedItem = parentMenu.SelectedItem;
        if(selectedItem != null && selectedItem.IsType(ItemType.Weapon))
        {
            result = new UnitWeaponCombiner(parentMenu.UnitData, selectedItem.WeaponData).CanUse;
        }
        return result;
    }

    public enum State
    {
        NoItem,
        Equip_Yes,
        Equip_PrimaryConflict,
        Equip_SecondaryConflict,
        Equip_Impossible,
        Equip_RequirementsNotMet,
        Unequip_InventoryFull,
        Unequip_Yes
    }

    static bool IsEquipState(State state)
    {
        return
            state == State.Equip_Impossible ||
            state == State.Equip_PrimaryConflict ||
            state == State.Equip_RequirementsNotMet ||
            state == State.Equip_SecondaryConflict ||
            state == State.Equip_Yes;
    }

    static bool IsDisabledState(State state)
    {
        return
            state == State.Equip_Impossible ||
            state == State.Equip_RequirementsNotMet ||
            state == State.Unequip_InventoryFull;
    }
}
