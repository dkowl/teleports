using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Skill {

	override public void internalCast(Unit caster, TargetInfo target)
    {
        target.unit.receiveDamage(caster.Damage, caster);
    }
}
