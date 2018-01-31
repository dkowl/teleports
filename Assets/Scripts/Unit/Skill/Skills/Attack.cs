using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class Attack : Skill {

    [FormerlySerializedAs("damageMultiplier_")]
    [SerializeField] private float damageMultiplier = 1;
    [SerializeField] private AttackDamageType damageType;
    [SerializeField, ShowIf("ShowDamageRanges")] private int minDamage;
    [SerializeField, ShowIf("ShowDamageRanges")] private int maxDamage;


    override public void CastInternal(Unit caster, List<CastTarget> targets)
    {
        foreach (var target in targets)
        {
            float damage = Damage(caster) * damageMultiplier;

            target.Unit.ReceiveDamage(damage, caster);

            //physics pushback
            Vector3 directionVector = target.Unit.transform.position - caster.transform.position;
            directionVector.Normalize();
            Vector3 forceVector = directionVector * damage * 10;
            Vector3 randomOffset = Random.insideUnitSphere;
            target.Unit.Rigidbody.AddForceAtPosition(forceVector, target.Unit.Rigidbody.position + randomOffset, ForceMode.Impulse);
        }
    }

    protected AttackDamageType DamageType
    {
        get { return damageType; }
    }

    protected virtual float Damage(Unit caster)
    {
        switch (damageType)
        {
            case AttackDamageType.RawDamage:
                return Random.Range(minDamage, maxDamage);
            case AttackDamageType.Weapon:
                return caster.Damage;
        }
        return 0;
    }

    private bool ShowDamageRanges()
    {
        return damageType == AttackDamageType.RawDamage;
    }
}

public enum AttackDamageType
{
    Weapon,
    RawDamage
}