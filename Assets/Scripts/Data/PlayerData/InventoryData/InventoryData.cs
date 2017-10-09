using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

using ItemID = System.String;

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

        Debug.Log("Inventory is full!");
        return;
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

    //public void Equip(Item)
   
}
