using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeSlot : UnlockableSlot {

    [SerializeField] private int skillTreeId = 0;

    public int SkillTreeId
    {
        get
        {
            return skillTreeId;
        }
    }

    public void setTree(int id)
    {
        if(Fill())
        {
            skillTreeId = id;
        }
    }	
}
