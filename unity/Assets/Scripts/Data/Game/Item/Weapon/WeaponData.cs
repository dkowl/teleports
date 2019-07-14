using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public partial class WeaponData {

    //temporary
    public void CorrectInvalidData()
    {

    }

   
    public float MinDamage => Damage * (1.0f - DamageSpread);
    public float MaxDamage => Damage * (1.0f + DamageSpread);
    public float AverageDamage => (float)(MinDamage + MaxDamage) / 2;

    public float TotalAttackTime => 1 / AttacksPerSecond;

    public float AttacksPerSecond => Main.StaticData.Game.Skills.GetValue(BasicSkillId).Data.AttacksPerSecond * SpeedModifier;
}
