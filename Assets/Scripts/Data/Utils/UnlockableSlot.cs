using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockableSlot {

    [SerializeField] protected bool isEmpty = true;
    [SerializeField] protected bool isLocked = true;

    public bool IsEmpty
    {
        get { return isEmpty; }
    }

    public bool IsLocked
    {
        get { return isLocked; }
    }

    public void Unlock()
    {
        isLocked = false;
    }

    //returns false when filling fails
    //otherwise fills and returns true
    public bool Fill()
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
}
