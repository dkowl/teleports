using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifier : Perk {

    public float bonus_, multiplier_;

    public override void applyInternal(Unit target)
    {
        base.applyInternal(target);

        target.damage_.modify(bonus_, multiplier_);
    }
}
