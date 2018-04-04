using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;

public partial class WeaponData {

    //temporary
    public void CorrectInvalidData()
    {

    }


    public float MinDamage => Damage * (1.0f - DamageSpread);
    public float MaxDamage => Damage * (1.0f + DamageSpread);
    public float AverageDamage => (float)(MinDamage + MaxDamage) / 2;

    public float TotalAttackTime => 1;

    public float AttacksPerSecond => 1 / TotalAttackTime;
}
