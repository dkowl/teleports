using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class DeadState : ActionState {

    protected Unit lastAttacker;

	public DeadState(Unit unit) : base(unit)
    {
        Reset();
    }

    protected override void OnStart()
    {
        if (!IsActive)
        {
            if (lastAttacker != null)
            {
                XpComponent xp = lastAttacker.gameObject.GetComponent<XpComponent>();
                if (xp != null)
                {
                    xp.ReceiveXp((1000 * Unit.UnitData.Level));
                }
            }
            Unit.Graphics.SwitchToRagdoll();
        }
    }

    protected override void OnReset()
    {
        lastAttacker = null;
    }

    public void StartDeath()
    {
        Start();
    }

    public Unit LastAttacker
    {
        get { return lastAttacker; }
        set { lastAttacker = value; }
    }
}
