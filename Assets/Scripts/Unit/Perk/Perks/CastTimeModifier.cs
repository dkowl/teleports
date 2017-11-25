using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTimeModifier : Perk {

    public Skill skillToModify;
    public float castTimeModifier = 1f;

    public override void Apply(Unit target)
    {
        base.Apply(target);

        foreach(Skill skill in target.skills)
        {
            if(skill.name == skillToModify.name)
            {
                skill.ModifyAttribute(Skill.AttributeType.CastTime, 0, castTimeModifier);
            }
        }
    }
}
