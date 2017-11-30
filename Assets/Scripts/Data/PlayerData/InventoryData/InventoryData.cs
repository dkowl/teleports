using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

[System.Serializable]
public class InventoryData {
    
    [SerializeField] private int maxItems = 32;
    [SerializeField] private List<InventorySlot> invSlots;
    [SerializeField] private ItemID[] eq = new ItemID[Utils.EnumCount(typeof(EquipmentSlot))];

    public InventoryData()
    {
        Initialize();
    }

    public void Initialize()
    {
        maxItems = 32;
        Utils.InitWithValues(ref invSlots, maxItems, new InventorySlot());
        eq = new ItemID[Utils.EnumCount(typeof(EquipmentSlot))];
    }

    public void Add(ItemID id)
    {
        //stack on existing slot
        foreach(InventorySlot slot in invSlots)
        {
            if(!slot.Empty && slot.itemID == id)
            {
                slot.Add(id);
                return;
            }
        }

        //add to empty slot
        foreach(InventorySlot slot in invSlots)
        {
            if (slot.Empty)
            {
                slot.Add(id);
                return;
            }
        }

        //add extra slot if possible
        if(invSlots.Count < maxItems)
        {
            InventorySlot slot = new InventorySlot();
            slot.Add(id);
            invSlots.Add(slot);
        }

        Debug.Log("Inventory is full!");
        return;
    }

    public bool Contains(ItemID id)
    {
        foreach(InventorySlot slot in invSlots)
        {
            if (slot.itemID == id) return true;
        }
        return false;
    }

    public void Remove(ItemID id)
    {
        foreach (InventorySlot slot in invSlots)
        {
            if (!slot.Empty && slot.itemID == id)
            {
                slot.Pop();
                return;
            }
        }
    }

    public void Equip(ItemID id)
    {
        if (Contains(id))
        {
            ItemData item = MainData.CurrentGameData.GetItem(id);
            int slotID = (int)item.Slot;
            Unequip(item.Slot);
            Remove(id);
            eq[slotID] = id;
        }
    }

    public void Unequip(EquipmentSlot slot)
    {
        int slotID = (int)slot;
        if (eq[slotID] != null)
        {
            Add(eq[slotID]);
            eq[slotID] = null;
        }
    }

    public List<ItemData> GetEquippedItems()
    {
        return GetItems(eq);
    }

    public List<ItemData> GetAllItemsInInventory()
    {
        List<ItemID> ids = new List<ItemID>();
        foreach(var slot in invSlots)
        {
            if(!slot.Empty)
            {
                ids.Add(slot.itemID);
            }
        }
        return GetItems(ids);
    }

    public void CorrectInvalidData()
    {
        if(maxItems == 0)
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
