using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingState : ActionState {

    protected Quaternion rotationTarget;

    public RotatingState(Unit unit) : base(unit)
    {        
    }

    protected override void OnUpdate(float dTime)
    {
        Unit.transform.rotation = Quaternion.RotateTowards(Unit.transform.rotation, rotationTarget, dTime * Unit.RotationSpeed * 360);
        if (Unit.transform.rotation == rotationTarget)
        {
            Reset();
        }        
    }

    public Quaternion RotationTarget
    {
        get { return rotationTarget; }
        set
        {
            if(value != rotationTarget && value != Unit.transform.rotation)
            {
                rotationTarget = value;
                rotationTarget = Quaternion.Euler(0, rotationTarget.eulerAngles.y, 0);
                Start();
            }
        }
    }
}
