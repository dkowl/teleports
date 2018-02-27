using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AttackData {

    public void PopulateFromAttack(Attack attack)
    {
        damageMultiplier = attack.DamageMultiplier;
        damageType = attack.DamageType;
        minDamage = attack.MinDamage;
        maxDamage = attack.MaxDamage;
    }

    private bool ShowDamageRanges()
    {
        return damageType == AttackDamageType.RawDamage;
    }
}
