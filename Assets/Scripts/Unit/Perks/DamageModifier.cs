using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifier : Perk {

    public float bonus_ = 0, multiplier_ = 1;

    public override void apply(Unit target)
    {
        base.apply(target);

        target.damage_.modify(bonus_, multiplier_);
    }
}
