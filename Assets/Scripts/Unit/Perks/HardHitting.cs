using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardHitting : Perk {

    public float damageBonus_;

    public override void applyInternal(Unit target)
    {
        target.damage_.addBonus(damageBonus_);
    }
}
