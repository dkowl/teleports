using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Attack : Skill {

    [FormerlySerializedAs("damageMultiplier_")]
    [SerializeField]
    private float damageMultiplier = 1;

	override public void InternalCast(Unit caster, TargetInfo target)
    {
        target.TargetUnit.ReceiveDamage(caster.Damage * damageMultiplier, caster);
    }

    override public void InternalCast(Unit caster, List<CastTarget> targets)
    {
        foreach (var target in targets)
        {
            target.Unit.ReceiveDamage(caster.Damage * damageMultiplier, caster);
        }
    }
}
