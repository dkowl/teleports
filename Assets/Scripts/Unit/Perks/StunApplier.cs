using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunApplier : Perk {

    public float stunDuration_, stunChance_;

    public override void onCast(Unit caster, Skill skill, Skill.TargetInfo target)
    {
        base.onCast(caster, skill, target);

        if(stunChance_ > Random.Range(0f, 1f))
        {
            target.unit.stun(stunDuration_);
        }
    }
}
