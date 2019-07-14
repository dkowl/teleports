using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockableSlot : IDeepCopyable {

    [SerializeField] protected bool isEmpty = true;
    [SerializeField] protected bool isLocked = true;

    public UnlockableSlot()
    {
        isEmpty = true;
        isLocked = true;
    }

    public UnlockableSlot(UnlockableSlot other)
    {
        isEmpty = other.IsEmpty;
        isLocked = other.IsLocked;
    }

    public virtual object DeepCopy()
    {
        return new UnlockableSlot(this);
    }

    public void Unlock()
    {
        isLocked = false;
    }

    //returns false when filling fails
    //otherwise fills and returns true
    public bool TryFill()
    {
        if (!isLocked)
        {
            isEmpty = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsEmpty => isEmpty;
    public bool IsLocked => isLocked;
}
