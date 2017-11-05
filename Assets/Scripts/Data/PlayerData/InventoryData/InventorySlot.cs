using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemID = System.String;

[System.Serializable]
public struct InventorySlot
{
    [SerializeField] private ItemID id;
    [SerializeField] private int count;

    public bool Empty
    {
        get { return count == 0; }
    }

    public ItemID itemID
    {
        get
        {
            if (!Empty)
            {
                return id;
            }
            else
            {
                return null;
            }
        }
    }

    public void Add(ItemID id)
    {
        if (this.id == id)
        {
            count++;
        }
        else
        {
            this.id = id;
            count = 1;
        }
    }

    public void Pop()
    {
        if (!Empty)
        {
            count--;
        }
    }
}
