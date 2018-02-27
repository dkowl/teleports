using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class Attack : Skill {

    public override void CastInternal(Unit caster, List<CastTarget> targets)
    {
        foreach (var target in targets)
        {
            float damage = Damage(caster) * DamageMultiplier;

            target.Unit.ReceiveDamage(damage, caster);

            //physics pushback
            Vector3 directionVector = target.Unit.transform.position - caster.transform.position;
            directionVector.Normalize();
            Vector3 forceVector = directionVector * damage;
            Vector3 randomOffset = Random.insideUnitSphere / 2;
            target.Unit.Rigidbody.AddForceAtPosition(forceVector, target.Unit.Rigidbody.position + randomOffset, ForceMode.Impulse);
        }
    }

    public override float GetReach(Unit caster)
    {
        float result = base.GetReach(caster);
        if(DamageType == AttackDamageType.Weapon)
        {
            result += caster.WeaponCombiner.WeaponReach;
        }
        return result;
    }

    protected virtual float Damage(Unit caster)
    {
        switch (DamageType)
        {
            case AttackDamageType.RawDamage:
                return Random.Range(MinDamage, MaxDamage);
            case AttackDamageType.Weapon:
                return caster.Damage;
        }
        return 0;
    }

    private bool ShowDamageRanges()
    {
        return DamageType == AttackDamageType.RawDamage;
    }

    public float DamageMultiplier { get { return Data.AttackData.DamageMultiplier; } }
    public int MinDamage { get { return Data.AttackData.MinDamage;} }
    public int MaxDamage { get { return Data.AttackData.MaxDamage;} }

    public AttackDamageType DamageType
    {
        get { return Data.AttackData.DamageType; }
    }
}

public enum AttackDamageType
{
    Weapon,
    RawDamage
}