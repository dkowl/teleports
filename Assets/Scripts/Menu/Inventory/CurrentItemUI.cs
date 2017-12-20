using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class CurrentItemUI : LoadableBehaviour {

	[SerializeField] private InventoryMenu parentMenu;
    [SerializeField] private RawImage itemPreview;
    [SerializeField] private RawImage itemPreviewLockImage;
    [SerializeField] private Text itemName;
    [SerializeField] private ItemDescriptionUI itemDescription;

    protected override void LoadDataInternal()
    {
        if (parentMenu == null)
            parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);

        ItemData currentItem = parentMenu.SelectedItem;
        itemDescription.ItemData = currentItem;

        if (currentItem != null)
        {
            bool locked = false;
            if (currentItem.IsType(ItemType.Weapon))
            {
                UnitData unitData = parentMenu.UnitData;
                UnitWeaponCombiner combiner = new UnitWeaponCombiner(unitData, currentItem.WeaponData);
                locked = !combiner.CanUse;
            }
            itemPreview.enabled = true;
            if (locked)
            {
                itemPreviewLockImage.enabled = true;
            }
            else
            {
                itemPreviewLockImage.enabled = false;
            }
            itemName.text = currentItem.DisplayName;
        }
        else
        {
            itemPreview.enabled = false;
            itemPreviewLockImage.enabled = false;
            itemName.text = "";
        }
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        InventoryMenu.OnSelectionChangedEvent += LoadData;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        InventoryMenu.OnSelectionChangedEvent -= LoadData;
    }
}
