using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using System;

public class InventorySlotUI : LoadableBehaviour {

    private InventoryMenu parentMenu;
    private int slotId;
    private bool isInitialized;

    [SerializeField] private RawImage itemIcon;
    [SerializeField] private Text countText;

    protected override void LoadDataInternal()
    {
        if (!isInitialized) return;

        InventoryData inventoryData = parentMenu.InventoryData;
        InventorySlotData inventorySlotData = inventoryData.GetInventorySlotData(slotId);

        if (!inventorySlotData.Empty)
        {
            itemIcon.enabled = true;
            ItemData itemData = inventorySlotData.Item;

            itemIcon.texture = parentMenu.ItemIconAtlas;
            itemIcon.uvRect = parentMenu.GetItemIconUvRect(itemData);
        }
        else
        {
            itemIcon.enabled = false;
        }

        int count = inventorySlotData.Count;
        if (count > 1)
            countText.text = count.ToString();
        else
            countText.text = "";
    }

    public void Initialize(InventoryMenu parentMenu, int slotId)
    {
        if (!isInitialized)
        {
            this.parentMenu = parentMenu;
            this.slotId = slotId;
            isInitialized = true;
            LoadData();
        }
    }
}
