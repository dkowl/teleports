using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class CurrentItemUI : LoadableBehaviour {

	[SerializeField] private InventoryMenu parentMenu;
    [SerializeField] private RawImage itemPreview;
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
            itemPreview.enabled = true;
            itemName.text = currentItem.DisplayName;
        }
        else
        {
            itemPreview.enabled = false;
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
