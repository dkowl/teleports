using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class InventorySlotData : IDeepCopyable
{
    [SerializeField, HideIf("Empty")] private ItemData item;
    [SerializeField] private int count;
    [SerializeField, HideInInspector] private int maxCount;

    public InventorySlotData()
    {
        item = null;
        count = 0;
        maxCount = 99;
    }

    public InventorySlotData(InventorySlotData other)
    {
        item = other.item;
        count = other.count;
        maxCount = other.maxCount;
    }

    public InventorySlotData(int maxCount)
    {
        item = null;
        count = 0;
        this.maxCount = maxCount;
    }

    public object DeepCopy()
    {
        return new InventorySlotData(this);
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

    public int Count
    {
        get { return count; }
    }

    public bool Empty
    {
        get { return count == 0; }
    }
}
