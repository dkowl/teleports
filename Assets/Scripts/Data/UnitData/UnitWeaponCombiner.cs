using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitWeaponCombiner {

    [SerializeField] private bool canUse;
    [SerializeField] private int bonusStr;
    [SerializeField] private int bonusDex;
    [SerializeField] private int bonusInt;
    [SerializeField] private DamageBonus damageBonus;
    [SerializeField] private SpeedBonus speedBonus;
    [SerializeField] private ReachBonus reachBonus;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float weaponReach;
    [SerializeField] private float totalReach;
    [SerializeField] private float castTime;
    [SerializeField] private float afterCastLockTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float attacksPerSecond;
    [SerializeField] private float damagePerSecond;

    public UnitWeaponCombiner(UnitData unit, WeaponData weapon)
    {
        if (weapon == null || unit == null) return;

        canUse = 
            unit.Abilities.Strength >= weapon.StrRequired &&
            unit.Abilities.Dexterity >= weapon.DexRequired &&
            unit.Abilities.Intelligence >= weapon.IntRequired;
        
        bonusStr = Mathf.Max(0, unit.Abilities.Strength - weapon.StrRequired);
        bonusDex = Mathf.Max(0, unit.Abilities.Dexterity - weapon.DexRequired);
        bonusInt = Mathf.Max(0, unit.Abilities.Intelligence - weapon.IntRequired);
        damageBonus = new DamageBonus(weapon, bonusStr, bonusDex, bonusInt);
        speedBonus = new SpeedBonus(weapon, bonusStr, bonusDex, bonusInt, weapon.CastTime + weapon.AfterCastLockTime);
        reachBonus = new ReachBonus(weapon, bonusStr, bonusDex, bonusInt);
        minDamage = weapon.MinDamage + (int)damageBonus.Value;
        maxDamage = weapon.MaxDamage + (int)damageBonus.Value;
        weaponReach = weapon.Reach + reachBonus.Value;
        totalReach = weaponReach + MainData.CurrentGameData.GetSkill(unit.MainAttack).Reach;
        castTime = weapon.CastTime * speedBonus.Multiplier;
        afterCastLockTime = weapon.AfterCastLockTime * speedBonus.Multiplier;
        attackTime = castTime + afterCastLockTime;
        attacksPerSecond = 1 / attackTime;
        damagePerSecond = ((float)(minDamage + maxDamage) / 2) / attackTime;        
    }

    public int DamageRoll
    {
        get { return Random.Range(minDamage, maxDamage); }
    }

    public bool CanUse
    {
        get { return canUse; }
    }

    public DamageBonus DamageBonusData
    {
        get { return damageBonus; }
    }

    public SpeedBonus SpeedBonusData
    {
        get { return speedBonus; }
    }

    public ReachBonus ReachBonusData
    {
        get { return reachBonus; }
    }

    public int MinDamage
    {
        get { return minDamage; }
    }

    public int MaxDamage
    {
        get { return maxDamage; }
    }

    public float WeaponReach
    {
        get { return weaponReach; }
    }

    public float AttacksPerSecond
    {
        get { return attacksPerSecond; }
    }

    public float DamagePerSecond
    {
        get { return damagePerSecond; }
    }

    public class AbilityStatBonus
    {
        protected float strComponent;
        protected float dexComponent;
        protected float intComponent;
        protected float value;

        public float StrComponent
        {
            get { return strComponent; }
        }

        public float DexComponent
        {
            get { return dexComponent; }
        }

        public float IntComponent
        {
            get { return intComponent; }
        }

        public float Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return string.Format(
                "{0} - Str: {1}, Dex: {2}, Int: {3}, Total: {4}\n",
                base.ToString(),
                strComponent,
                dexComponent,
                intComponent,
                value
                );
        }
    }

    public class DamageBonus : AbilityStatBonus
    {
        public DamageBonus(WeaponData weapon, int bonusStr, int bonusDex, int bonusInt)
        {
            strComponent = weapon.StrDamageBonus * bonusStr;
            dexComponent = weapon.DexDamageBonus * bonusDex;
            intComponent = weapon.IntDamageBonus * bonusInt;
            value = strComponent + dexComponent + intComponent;
        }
    }

    public class SpeedBonus : AbilityStatBonus
    {
        public static readonly float maxMultiplier = 0.5f;

        private float multiplier;

        public SpeedBonus(WeaponData weapon, int bonusStr, int bonusDex, int bonusInt, float attackTime)
        {
            value = 0;
            multiplier = 1;
            int[] abilityBonuses = { bonusStr, bonusDex, bonusInt };
            float[] weaponBonuses = { weapon.StrSpeedBonus, weapon.DexSpeedBonus, weapon.IntSpeedBonus };
            float[] multipliers = new float[abilityBonuses.Length];
            float[] absoluteDeltas = new float[abilityBonuses.Length];
            float[] perSecondDeltas = new float[abilityBonuses.Length];
            for (int i = 0; i<abilityBonuses.Length; i++)
            {
                multipliers[i] = Mathf.Sqrt(Mathf.Pow(1 - weaponBonuses[i], abilityBonuses[i]));
                absoluteDeltas[i] = (multiplier - multiplier * multipliers[i]) * (1 - maxMultiplier) * attackTime;
                float currentAttackTime = attackTime * multiplier;
                perSecondDeltas[i] = (1 / (currentAttackTime * multipliers[i])) - (1 / currentAttackTime);
                value += perSecondDeltas[i];
                multiplier *= multipliers[i];
            }
            strComponent = perSecondDeltas[0];
            dexComponent = perSecondDeltas[1];
            intComponent = perSecondDeltas[2];
        }

        public float Multiplier
        {
            get { return multiplier; }
        }
    }

    public class ReachBonus : AbilityStatBonus
    {
        public ReachBonus(WeaponData weapon, int bonusStr, int bonusDex, int bonusInt)
        {
            strComponent = weapon.StrReachBonus * bonusStr;
            dexComponent = weapon.DexReachBonus * bonusDex;
            intComponent = weapon.IntReachBonus * bonusInt;
            value = strComponent + dexComponent + intComponent;
        }
    }	
}
