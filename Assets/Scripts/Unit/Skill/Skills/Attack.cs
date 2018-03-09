using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class Attack : Skill {

    protected override void CastInternal(Unit caster, List<CastTarget> targets)
    {
        foreach (var target in targets)
        {
            float damage = Damage(caster) * DamageMultiplier;

            target.Unit.ReceiveDamage(damage, caster);

            //physics pushback
            target.Unit.Physics.ApplyForce(caster.transform, damage * Data.AttackData.PushbackFactor + Data.AttackData.BasePushabck);
        }
    }

    public override float GetReach(Unit caster)
    {
        //Debug.Log("Calling Attack.GetReach()");
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