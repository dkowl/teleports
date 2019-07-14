using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTimeModifier : Perk {

    [SerializeField] protected Skill skillToModify;
    [SerializeField] protected float castTimeModifier = 1f;

    protected override void ApplyInternal(Unit target)
    {
        foreach(Skill skill in target.Skills)
        {
            if(skill.name == skillToModify.name)
            {
                skill.ModifyAttribute(SkillData.AttributeType.CastTime, 0, castTimeModifier);
            }
        }
    }

    protected override void UnapplyInternal(Unit target)
    {
        foreach (Skill skill in target.Skills)
        {
            if (skill.name == skillToModify.name)
            {
                skill.ModifyAttribute(SkillData.AttributeType.CastTime, 0, 1/castTimeModifier);
            }
        }
    }
}
