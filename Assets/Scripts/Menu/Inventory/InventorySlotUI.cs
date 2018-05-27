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
    private string eqSlotName;
    private bool isInitialized;

    [SerializeField] private RawImage itemIcon;
    [SerializeField] private Image borderImage;
    [SerializeField] private Text countText;
    [SerializeField] private Text nameText;
    [SerializeField] private RawImage lockIcon;
    [SerializeField] private Color selectedBorderColor;
    [SerializeField] private Color deselectedBorderColor;

    protected override void LoadDataInternal()
    {
        if (!isInitialized)
        {
            return;
        }

        InventoryData inventoryData = parentMenu.InventoryData;
        ItemData itemData = null;
        int count = 0;
        bool itemIconEnabled = false;
        bool locked = false;

        if (!slotId.isEquipmentSlot)
        {
            InventorySlotData inventorySlotData = inventoryData.GetInventorySlotData(slotId.inventorySlotId);
            if (!inventorySlotData.Empty)
            {
                itemData = inventorySlotData.Item;
                itemIconEnabled = true;
            }
            count = inventorySlotData.Count;
        }
        else if (slotId.isEquipmentSlot)
        {
            EquipmentSlotData eqSlotData = inventoryData.EquipmentData.GetEquipmentSlot(slotId.equipmentSlotType);
            if (!eqSlotData.Empty)
            {
                itemData = eqSlotData.Item;
                if (eqSlotData.Primary)
                {
                    itemIconEnabled = true;
                }
                else
                {
                    locked = true;
                }
            }
        }

        if(itemData != null)
        {
            if (itemData.IsType(ItemType.Weapon))
            {
                UnitWeaponCombiner combiner = new UnitWeaponCombiner(parentMenu.UnitData, itemData.WeaponData);
                if (!combiner.CanUse)
                {
                    locked = true;
                }
            }
        }

        if (itemIconEnabled)
        {
            itemIcon.enabled = true;
            itemIcon.texture = parentMenu.ItemIconAtlas;
            itemIcon.uvRect = parentMenu.GetItemIconUvRect(itemData);
        }
        else
        {
            itemIcon.enabled = false;
        }

        lockIcon.enabled = locked;

        if (count > 1)
            countText.text = count.ToString();
        else
            countText.text = "";

        nameText.text = eqSlotName;

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
        InventoryMenu.UpdateUiEvent += LoadData;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        InventoryMenu.UpdateUiEvent -= LoadData;
    }

    public void Initialize(InventoryMenu parentMenu, SlotID slotId, string eqSlotName = "")
    {
        if (!isInitialized)
        {
            this.parentMenu = parentMenu;
            this.slotId = slotId;
            this.eqSlotName = eqSlotName;
            isInitialized = true;
            LoadData();
        }
    }
    
    public void Select()
    {
        parentMenu.Select(slotId);
    }
}
