using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public class InventoryData {
    
    [SerializeField] private int maxSlots = 32;
    [SerializeField] private EquipmentData equipmentData;
    [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 8)] private List<InventorySlot> invSlots;

    public InventoryData()
    {
        Initialize();
    }

    public void Initialize()
    {
        maxSlots = 32;
        Utils.InitWithValues(ref invSlots, maxSlots, new InventorySlot());
    }

    public void Add(ItemData item)
    {
        //stack on existing slot
        foreach(InventorySlot slot in invSlots)
        {
            if(!slot.Empty && slot.Item == item)
            {
                slot.Add(item);
                return;
            }
        }

        //add to empty slot
        foreach(InventorySlot slot in invSlots)
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
            InventorySlot slot = new InventorySlot();
            slot.Add(item);
            invSlots.Add(slot);
        }

        Debug.Log("Inventory is full!");
        return;
    }

    public bool Contains(ItemData item)
    {
        foreach(InventorySlot slot in invSlots)
        {
            if (slot.Item == item) return true;
        }
        return false;
    }

    public void Remove(ItemData item)
    {
        foreach (InventorySlot slot in invSlots)
        {
            if (!slot.Empty && slot.Item == item)
            {
                slot.Pop();
                return;
            }
        }
    }

    public void Equip(ItemData item)
    {
        if (Contains(item))
        {
            Remove(item);
            equipmentData.GetEquipmentSlot(item.PrimarySlot).Equip(item);
        }
    }

    public void Unequip(EquipmentSlotType slot)
    {
        ItemData unequippedItem = equipmentData.Unequip(slot);
        Add(unequippedItem);
    }

    public List<ItemData> GetEquippedItems()
    {
        return equipmentData.GetEquippedItems();
    }

    public List<ItemData> GetAllItemsInInventory()
    {
        List<ItemData> result = new List<ItemData>();
        foreach(var slot in invSlots)
        {
            if(!slot.Empty)
            {
                result.Add(slot.Item);
            }
        }
        return result;
    }

    public void CorrectInvalidData()
    {
        if(maxSlots == 0)
        {
            Initialize();
        }
    }

    List<ItemData> GetItems(IEnumerable<ItemID> itemIds)
    {
        List<ItemData> result = new List<ItemData>();

        foreach (ItemID id in itemIds)
        {
            ItemData itemData = MainData.CurrentGameData.GetItem(id);
            if (itemData != null)
            {
                result.Add(itemData);
            }

        }
        return result;
    }
   
}
