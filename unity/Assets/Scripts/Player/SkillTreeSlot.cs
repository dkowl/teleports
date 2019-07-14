using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeSlot : UnlockableSlot {

    [SerializeField] private int skillTreeId = 0;

    public SkillTreeSlot()
    {
        skillTreeId = 0;
    }

    public SkillTreeSlot(SkillTreeSlot other) : base(other)
    {
        skillTreeId = other.skillTreeId;
    }

    public override object DeepCopy()
    {
        return new SkillTreeSlot(this);
    }

    public int SkillTreeId => skillTreeId;

    public void SetTree(int id)
    {
        if(TryFill())
        {
            skillTreeId = id;
        }
    }	
}
