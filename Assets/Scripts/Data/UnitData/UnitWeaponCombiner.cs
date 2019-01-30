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
    [SerializeField] private float attackSpeedModifier;
    [SerializeField] private float attacksPerSecond;
    [SerializeField] private float damagePerSecond;

    public UnitWeaponCombiner(UnitData unit, WeaponData weapon)
    {
        if (weapon == null || unit == null) return;

        canUse = 
            unit.Attributes.Strength >= weapon.StrRequired &&
            unit.Attributes.Dexterity >= weapon.DexRequired &&
            unit.Attributes.Intelligence >= weapon.IntRequired;
        
        bonusStr = (int)Mathf.Max(0, unit.Attributes.Strength - weapon.StrRequired);
        bonusDex = (int)Mathf.Max(0, unit.Attributes.Dexterity - weapon.DexRequired);
        bonusInt = (int)Mathf.Max(0, unit.Attributes.Intelligence - weapon.IntRequired);
        damageBonus = new DamageBonus(weapon, bonusStr, bonusDex, bonusInt);
        speedBonus = new SpeedBonus(weapon, bonusStr, bonusDex, bonusInt, weapon.TotalAttackTime);
        reachBonus = new ReachBonus(weapon, bonusStr, bonusDex, bonusInt);
        minDamage = (int)weapon.MinDamage + (int)damageBonus.Value;
        maxDamage = (int)weapon.MaxDamage + (int)damageBonus.Value;
        weaponReach = weapon.Reach + reachBonus.Value;
        totalReach = weaponReach + Main.StaticData.Game.Skills.GetValue(unit.MainAttack).Data.Reach;
        attackSpeedModifier = weapon.SpeedModifier / speedBonus.Multiplier;
        attacksPerSecond = weapon.AttacksPerSecond / speedBonus.Multiplier;
        damagePerSecond = AverageDamage * AttacksPerSecond;
    }

    public int DamageRoll => Random.Range(minDamage, maxDamage);
    public bool CanUse => canUse;
    public DamageBonus DamageBonusData => damageBonus;
    public SpeedBonus SpeedBonusData => speedBonus;
    public ReachBonus ReachBonusData => reachBonus;
    public int MinDamage => minDamage;
    public int MaxDamage => maxDamage;
    public float WeaponReach => weaponReach;
    public float AttackSpeedModifier => attackSpeedModifier;
    public float AttacksPerSecond => attacksPerSecond;
    public float DamagePerSecond => damagePerSecond;
    public float AverageDamage => (minDamage + maxDamage) / 2.0f;

    public class AbilityStatBonus
    {
        protected float strComponent;
        protected float dexComponent;
        protected float intComponent;
        protected float value;

        public float StrComponent => strComponent;
        public float DexComponent => dexComponent;
        public float IntComponent => intComponent;
        public float Value => value;

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
        public static readonly float minMultiplier = 0.5f;

        private float multiplier;

        public SpeedBonus(WeaponData weapon, int bonusStr, int bonusDex, int bonusInt, float attackTime)
        {
            value = 0;
            multiplier = 1;
            int[] abilityBonuses = { bonusStr, bonusDex, bonusInt };
            float[] weaponBonuses = { weapon.StrSpeedBonus, weapon.DexSpeedBonus, weapon.IntSpeedBonus };
            float[] abilityContributionScores = new float[abilityBonuses.Length];
            float abilityContributionScoreTotal = 0;
            for (int i = 0; i<abilityBonuses.Length; i++)
            {
                multiplier *= Mathf.Pow(1 - weaponBonuses[i], abilityBonuses[i]);
                abilityContributionScores[i] = (1 / Mathf.Pow(1 - weaponBonuses[i], abilityBonuses[i])) - 1;
                abilityContributionScoreTotal += abilityContributionScores[i];
            }
            multiplier = Mathf.Sqrt(multiplier);
            multiplier = multiplier * (1 - minMultiplier) + minMultiplier;
            value = (1 / (attackTime * multiplier)) - (1 / attackTime);
            strComponent = value * (abilityContributionScores[0] / abilityContributionScoreTotal);
            dexComponent = value * (abilityContributionScores[1] / abilityContributionScoreTotal);
            intComponent = value * (abilityContributionScores[2] / abilityContributionScoreTotal);
        }

        public float Multiplier => multiplier;
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
