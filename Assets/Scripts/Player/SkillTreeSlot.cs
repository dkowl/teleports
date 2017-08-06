using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeSlot  {

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isEmpty = true;
    [SerializeField] private int skillTreeId = 0;

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }
    }

    public int SkillTreeId
    {
        get
        {
            return skillTreeId;
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void setTree(int id)
    {
        if(isUnlocked && isEmpty)
        {
            skillTreeId = id;
            isEmpty = false;
        }
    }	
}
