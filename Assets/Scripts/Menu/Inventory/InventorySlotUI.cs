using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using SlotID = InventoryMenu.ItemSlotID;

public class InventorySlotUI : LoadableBehaviour {

    private InventoryMenu parentMenu;
    private SlotID slotId;
    private bool isInitialized;

    [SerializeField] private RawImage itemIcon;
    [SerializeField] private Image borderImage;
    [SerializeField] private Text countText;
    [SerializeField] private RawImage lockIcon;
    [SerializeField] private Color selectedBorderColor;
    [SerializeField] private Color deselectedBorderColor;

    protected override void LoadDataInternal()
    {
        if (!isInitialized) return;

        InventoryData inventoryData = parentMenu.InventoryData;
        if (!slotId.isEquipmentSlot)
        {
            InventorySlotData inventorySlotData;
            inventorySlotData = inventoryData.GetInventorySlotData(slotId.inventorySlotId);

            if (!inventorySlotData.Empty)
            {
                itemIcon.enabled = true;
                ItemData itemData = inventorySlotData.Item;
                Debug.Assert(itemData != null);
                bool locked = false;
                if (itemData.IsType(ItemType.Weapon)) {
                    UnitWeaponCombiner combiner = new UnitWeaponCombiner(parentMenu.UnitData, itemData.WeaponData);
                    locked = !combiner.CanUse;
                }
            
                itemIcon.texture = parentMenu.ItemIconAtlas;
                itemIcon.uvRect = parentMenu.GetItemIconUvRect(itemData);
                lockIcon.enabled = locked;
            }
            else
            {
                itemIcon.enabled = false;
                lockIcon.enabled = false;
            }

            int count = inventorySlotData.Count;
            if (count > 1)
                countText.text = count.ToString();
            else
                countText.text = "";
        }

        if (parentMenu.IsSelected(slotId))
        {
            borderImage.color = selectedBorderColor;
        }
        else
        {
            borderImage.color = deselectedBorderColor;
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

    public void Initialize(InventoryMenu parentMenu, SlotID slotId)
    {
        if (!isInitialized)
        {
            this.parentMenu = parentMenu;
            this.slotId = slotId;
            isInitialized = true;
            LoadData();
        }
    }
    
    public void Select()
    {
        parentMenu.Select(slotId);
    }
}
