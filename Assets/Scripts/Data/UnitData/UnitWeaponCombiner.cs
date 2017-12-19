using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponCombiner {

    private bool canUse;
    private int bonusStr;
    private int bonusDex;
    private int bonusInt;
    private DamageBonus damageBonus;
    private SpeedBonus speedBonus;
    private ReachBonus reachBonus;
    private int minDamage;
    private int maxDamage;
    private float weaponReach;
    private float totalReach;
    private float castTime;
    private float afterCastLockTime;
    private float attackTime;
    private float damagePerSecond;

    public UnitWeaponCombiner(UnitData unit, WeaponData weapon)
    {
        canUse = 
            unit.Abilities.Strength >= weapon.StrRequired &&
            unit.Abilities.Dexterity >= weapon.DexRequired &&
            unit.Abilities.Intelligence >= weapon.IntRequired;

        if (canUse)
        {
            bonusStr = unit.Abilities.Strength - weapon.StrRequired;
            bonusDex = unit.Abilities.Dexterity - weapon.DexRequired;
            bonusInt = unit.Abilities.Intelligence - weapon.IntRequired;
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
            damagePerSecond = ((float)(minDamage + maxDamage) / 2) / attackTime;
        }
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
            get { return (int)value; }
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
            int[] abilityBonuses = { bonusStr, bonusDex, bonusInt };
            float[] weaponBonuses = { weapon.StrSpeedBonus, weapon.DexSpeedBonus, weapon.IntSpeedBonus };
            float[] multipliers = new float[abilityBonuses.Length];
            float[] deltas = new float[abilityBonuses.Length];
            float multiplier = 1;
            for (int i = 0; i<abilityBonuses.Length; i++)
            {
                multipliers[i] = Mathf.Sqrt(Mathf.Pow(1 - weaponBonuses[i], abilityBonuses[i]));
                deltas[i] = multiplier - multiplier * multipliers[i] * (1 - maxMultiplier) * attackTime;
                value += deltas[i];
                multiplier *= multipliers[i];
            }
            strComponent = deltas[0];
            dexComponent = deltas[1];
            intComponent = deltas[2];
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
