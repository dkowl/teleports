using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class DeadState : ActionState {

    protected Unit lastAttacker = null;

	public DeadState(Unit unit) : base(unit)
    {
        Reset();
    }

    public override void Start()
    {
        if (!IsActive)
        {
            isActive = true;
            if (lastAttacker != null)
            {
                XpComponent xp = lastAttacker.gameObject.GetComponent<XpComponent>();
                if (xp != null)
                {
                    xp.ReceiveXp((1000 * unit.UnitData.Level));
                }
            }
            unit.Rigidbody.constraints = 0;
            unit.Rigidbody.useGravity = true;
            unit.gameObject.SetLayerIncludingChildren(0);
        }
    }

    public override void Update(float dTime)
    {
        
    }

    public override void Reset()
    {
        lastAttacker = null;
        isActive = false;
    }

    public Unit LastAttacker
    {
        get { return lastAttacker; }
        set { lastAttacker = value; }
    }
}
