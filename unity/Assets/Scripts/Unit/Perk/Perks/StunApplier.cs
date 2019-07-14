using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunApplier : Perk {

    [SerializeField] float stunDuration;
    [SerializeField] float stunChance;

    protected override void ApplyInternal(Unit target)
    {
        target.CastingState.castEvent += Stun;
    }

    protected override void UnapplyInternal(Unit target)
    {
        target.CastingState.castEvent -= Stun;
    }

    public void Stun(CastingState.CastEventArgs eventArgs)
    {
        Unit unit = eventArgs.TargetInfo.TargetUnit;
        if(Random.value < stunChance)
        {
            unit.Stun(stunDuration);
        }
    }
}
