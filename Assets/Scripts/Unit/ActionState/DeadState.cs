using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Xp xp = lastAttacker.gameObject.GetComponent<Xp>();
                if (xp != null)
                {
                    xp.receiveXp((1000 * unit.unitData.Level));
                }
            }
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
