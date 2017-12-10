using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class InventorySlotData
{
    [SerializeField, HideIf("Empty")] private ItemData item;
    [SerializeField] private int count;
    [SerializeField, HideInInspector] private int maxCount;

    public InventorySlotData()
    {
        item = null;
        count = 0;
    }

    public InventorySlotData(int maxCount)
    {
        item = null;
        count = 0;
        this.maxCount = maxCount;
    }

    public void Add(ItemData item)
    {
        if (this.item == item)
        {
            count++;
        }
        else
        {
            this.item = item;
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

    public void CorrectInvalidData()
    {
        if (Empty)
        {
            item = null;
        }
    }

    public bool Empty
    {
        get { return count == 0; }
    }

    public ItemData Item
    {
        get
        {
            if (!Empty)
            {
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}
