using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentSlotData {
    
    [SerializeField, HideIf("Empty"), HideLabel] private ItemData item;
    [SerializeField, HideIf("Empty")] private bool primary;
    [SerializeField, ShowIf("locked")] private bool locked;
    [SerializeField, ShowIf("Empty")] private bool empty;

    public EquipmentSlotData()
    {
        item = null;
        primary = true;
        locked = false;
        empty = true;
    }

    public void Equip(ItemData item, bool asPrimary)
    {
        if (!Locked)
        {
            if (!Empty)
                Debug.LogWarning("Slot full");
            else
            {
                this.item = item;
                primary = asPrimary;
                empty = false;
            }
        }
    }

    public void Unequip()
    {
        if (Empty)
            Debug.LogWarning("Unequipping empty slot");
        item = null;
        empty = true;
    }

    public ItemData UnequipAndReturn()
    {
        if (Empty)
        {
            Debug.LogWarning("Unequipping empty slot");
            return null;
        }

        ItemData unequippedItem = item;
        Unequip();
        return unequippedItem;
    }

    public ItemData Item
    {
        get
        {
            if (Empty || Locked)
                return null;
            else
                return item;
        }
    }

    public bool Primary
    {
        get { return primary; }
    }

    public bool Empty
    {
        get { return item == null || Locked || empty; }
    }

    public bool Locked
    {
        get { return locked; }
    }
    
}
