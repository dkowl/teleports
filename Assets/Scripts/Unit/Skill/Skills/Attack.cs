using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Attack : Skill {

    [FormerlySerializedAs("damageMultiplier_")]
    [SerializeField]
    private float damageMultiplier = 1;

    override public void InternalCast(Unit caster, List<CastTarget> targets)
    {
        foreach (var target in targets)
        {
            float damage = caster.Damage * damageMultiplier;

            target.Unit.ReceiveDamage(caster.Damage * damageMultiplier, caster);

            //physics pushback
            Vector3 directionVector = target.Unit.transform.position - caster.transform.position;
            directionVector.Normalize();
            Vector3 forceVector = directionVector * damage;
        }
    }
}
