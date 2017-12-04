using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentSlotData {

    [SerializeField, HideIf("locked"), HideLabel] private ItemData item;
    [SerializeField, ShowIf("locked")] private bool locked;

    public EquipmentSlotData()
    {
        item = null;
    }

    public void Equip(ItemData item)
    {
        if (!Locked)
        {
            if (!Empty)
                Debug.LogWarning("Slot full");
            else
                this.item = item;
        }
    }

    public void Unequip()
    {
        if (Empty)
            Debug.LogWarning("Unequipping empty slot");
        item = null;
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

    public bool Empty
    {
        get { return item != null || Locked; }
    }

    public bool Locked
    {
        get { return locked; }
    }
    
}
