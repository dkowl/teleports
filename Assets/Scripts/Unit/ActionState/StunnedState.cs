using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : ActionState {

    protected float stunTime;

    public StunnedState(Unit unit): base(unit)
    {
        Reset();
    }

    protected override void OnStart()
    {
        if(stunTime > 0)
        {
            Unit.Graphics.showMessage("Stunned!");
        }
    }

    protected override void OnUpdate(float dTime)
    {
        stunTime -= dTime;
        if(stunTime <= 0)
        {
            Reset();
        }
    }

    protected override void OnReset()
    {
        stunTime = 0;
    }

    public float StunTime
    {
        get { return stunTime; }
        set
        {
            if(value > stunTime)
            {
                stunTime = value;
                Start();
            }
        }
    }
}
