using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingState : ActionState {

    protected Quaternion rotationTarget;

    public RotatingState(Unit unit) : base(unit)
    {
        Reset();
    }

    public override void Start()
    {
        isActive = true;
    }

    public override void Update(float dTime)
    {
        if (IsActive && !IsBlocked)
        {
            unit.transform.rotation = Quaternion.RotateTowards(unit.transform.rotation, rotationTarget, dTime * unit.RotationSpeed * 360);
            if (unit.transform.rotation == rotationTarget)
            {
                Reset();
            }
        }
    }

    public override void Reset()
    {
        isActive = false;
        rotationTarget = unit.transform.rotation;
    }

    public Quaternion RotationTarget
    {
        get { return rotationTarget; }
        set
        {
            if(value != rotationTarget && value != unit.transform.rotation)
            {
                rotationTarget = value;
                rotationTarget = Quaternion.Euler(0, rotationTarget.eulerAngles.y, 0);
                Start();
            }
        }
    }
}
