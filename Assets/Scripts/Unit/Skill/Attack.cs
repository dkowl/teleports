using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Skill {

    public float damageMultiplier_;

	override public void internalCast(Unit caster, TargetInfo target)
    {
        target.unit.receiveDamage(caster.Damage * damageMultiplier_, caster);
    }
}
