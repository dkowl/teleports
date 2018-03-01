using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

public partial class InventoryData {

    public InventoryData()
    {
        Initialize();
    }

    public void Initialize()
    {
        maxSlots = 32;
        equipmentData = new EquipmentData();
        Utils.InitWithNew(ref invSlots, maxSlots);
    }

    public void CorrectInvalidData()
    {
        if (maxSlots == 0) Initialize();
        equipmentData.CorrectInvalidData();
        if(invSlots.Count != maxSlots)
        {
            Debug.LogWarning("Invalid inventory size '" + invSlots.Count.ToString() + "', changing to '" + maxSlots + "'");
            Utils.InitWithNew(ref invSlots, maxSlots);
        }
    }

    public bool CanAdd(ItemData item)
    {
        if(invSlots.Count < maxSlots)
        {
            return true;
        }
        else
        {
            foreach(var slot in invSlots)
            {
                if(slot.Empty || (!slot.Empty && slot.Item == item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanAdd(IList<ItemData> items)
    {
        int freeSlots = maxSlots - invSlots.Count;
        int neededSlots = items.Count(item => !Contains(item));
        return neededSlots <= freeSlots;
    }

    public void Add(ItemData item)
    {
        //stack on existing slot
        foreach(InventorySlotData slot in invSlots)
        {
            if(!slot.Empty && slot.Item == item)
            {
                slot.Add(item);
                return;
            }
        }

        //add to empty slot
        foreach(InventorySlotData slot in invSlots)
        {
            if (slot.Empty)
            {
                slot.Add(item);
                return;
            }
        }

        //add extra slot if possible
        if(invSlots.Count < maxSlots)
        {
            InventorySlotData slot = new InventorySlotData();
            slot.Add(item);
            invSlots.Add(slot);
        }

        Debug.Log("Inventory is full!");
    }

    public bool Contains(ItemData item)
    {
        foreach(InventorySlotData slot in invSlots)
        {
            if (slot.Item == item) return true;
        }
        return false;
    }

    public void Remove(ItemData item)
    {
        foreach (InventorySlotData slot in invSlots)
        {
            if (!slot.Empty && slot.Item == item)
            {
                slot.Pop();
                return;
            }
        }
    }

    public void Equip(string itemName)
    {
        Equip(GetItemByName(itemName));
    }

    public void Equip(ItemData item)
    {
        if (!Contains(item))
        {
            if (CanAdd(item)) Add(item);
            else return;
        }
        Equip(InventorySlotIdOf(item));
    }

    public void Equip(int inventorySlotId)
    {
        if (!IsValidSlotId(inventorySlotId)) return;

        InventorySlotData inventorySlot = invSlots[inventorySlotId];
        if (inventorySlot.Empty)
            return;

        ItemData item = inventorySlot.Item;
        EquipmentData.CanEquipResult canEquipResult = equipmentData.CanEquip(item);
        switch (canEquipResult.Status)
        {
            case EquipmentData.CanEquipStatus.Yes:
                Remove(item);
                equipmentData.Equip(item);
                break;
            case EquipmentData.CanEquipStatus.No_PrimaryConflict:
            case EquipmentData.CanEquipStatus.No_SecondaryConflict:
                Remove(item);
                IList<ItemData> unequippedItems = canEquipResult.ConflictingItems;
                foreach(var unequippedItem in unequippedItems)
                {
                    Add(unequippedItem);
                    equipmentData.Unequip(unequippedItem);
                }
                equipmentData.Equip(item);
                break;
        }
    }

    public CanUnequipStatus CanUnequip(EquipmentSlotType slot)
    {
        ItemData unequippedItem = equipmentData.GetEquipmentSlot(slot).Item;
        return CanAdd(unequippedItem) ? CanUnequipStatus.Yes : CanUnequipStatus.No_InventoryFull;
    }

    public void Unequip(EquipmentSlotType slot)
    {
        if (CanUnequip(slot) != CanUnequipStatus.Yes) return;
        ItemData unequippedItem = equipmentData.GetEquipmentSlot(slot).Item;
        Add(unequippedItem);
        equipmentData.Unequip(slot);
    }

    public InventorySlotData GetInventorySlotData(int inventorySlotId)
    {
        return IsValidSlotId(inventorySlotId) ? invSlots[inventorySlotId] : null;
    }

    public IList<EquipmentData.EquippedItemInfo> GetEquippedItems()
    {
        return equipmentData.GetEquippedItems();
    }

    public List<ItemData> GetAllItemsInInventory()
    {
        var result = new List<ItemData>();
        foreach(var slot in invSlots)
        {
            if(!slot.Empty)
            {
                result.Add(slot.Item);
            }
        }
        return result;
    }

    public IList<ItemData> GetAllItems()
    {
        var result = GetAllItemsInInventory();
        var result2 = GetEquippedItems().Select(slotInfo => slotInfo.Item).ToList();
        result.AddRange(result2);
        return result.AsReadOnly();
    }

    public int InventorySlotIdOf(ItemData item)
    {
        Debug.Assert(Contains(item));
        for (int i = 0; i < invSlots.Count; i++)
        {
            if (invSlots[i].Item == item)
            {
                return i;
            }
        }
        return -1;
    }

    private List<ItemData> GetItems(IEnumerable<ItemID> itemIds)
    {
        return itemIds.Select(id => MainData.Game.GetItem(id)).Where(itemData => itemData != null).ToList();
    }

    private ItemData GetItemByName(string name)
    {
        foreach(var invSlot in invSlots)
        {
            if(!invSlot.Empty && invSlot.Item.UniqueName == name)
            {
                return invSlot.Item;
            }
        }
        return null;
    }

    private bool IsValidSlotId(int id)
    {
        return id >= 0 && id < invSlots.Count;
    }

    public enum CanUnequipStatus
    {
        Yes,
        No_InventoryFull
    }
}
