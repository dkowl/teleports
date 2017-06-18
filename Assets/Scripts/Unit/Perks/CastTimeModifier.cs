using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTimeModifier : Perk {

    public Skill skillToModify_;
    public float castTimeModifier_ = 1f;

    public override void apply(Unit target)
    {
        base.apply(target);

        foreach(Skill skill in target.skills_)
        {
            if(skill.name == skillToModify_.name)
            {
                skill.castTime_.addMultiplier(castTimeModifier_);
            }
        }
    }
}
