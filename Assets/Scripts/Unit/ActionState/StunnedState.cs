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

    public override void Start()
    {
        if(stunTime > 0)
        {
            isActive = true;
        }
    }

    public override void Update(float dTime)
    {
        if(isActive && !IsBlocked)
        {
            stunTime -= dTime;
            if(stunTime <= 0)
            {
                Reset();
            }
        }
    }

    public override void Reset()
    {
        stunTime = 0;
        isActive = false;
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
