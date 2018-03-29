using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public partial class WeaponData {

    public WeaponData()
    {
    }

    public float AverageDamage => (float)(MinDamage + MaxDamage) / 2;

    public float TotalAttackTime => CastTime + AfterCastLockTime;

    public float AttacksPerSecond => 1 / TotalAttackTime;
}
