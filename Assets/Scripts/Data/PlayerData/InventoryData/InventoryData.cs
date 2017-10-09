using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

using ItemID = System.String;

[CreateAssetMenu(menuName = "Data/Inventoty/Data")]
public class InventoryData : ScriptableObject {
    
    [SerializeField] private int maxItems = 32;
    [SerializeField] private List<InventorySlot> invSlots;
    [SerializeField] private ItemID[] eq = new ItemID[Utils.EnumCount(typeof(EquipmentSlot))];

    public void Add(ItemID id)
    {
        foreach(InventorySlot slot in invSlots)
        {
            if(!slot.Empty && slot.itemID == id)
            {
                slot.Add(id);
                return;
            }
        }

        foreach(InventorySlot slot in invSlots)
        {
            if (slot.Empty)
            {
                slot.Add(id);
                return;
            }
        }

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
        List<ItemData> result = new List<ItemData>();

        foreach(ItemID id in eq)
        {
            ItemData itemData = MainData.CurrentGameData.GetItem(id);
            if(itemData != null)
            {
                result.Add(itemData);
            }
            
        }

        return result;
    }
   
}
