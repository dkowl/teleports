using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;
using System;

public class MovingState : ActionState {

    protected Vector3 moveDest;

    public MovingState(Unit unit) : base(unit)
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
            Vector3 offset = moveDest - unit.transform.position;

            if (unit.MoveSpeed * dTime < offset.magnitude)
            {
                offset *= unit.MoveSpeed * dTime / offset.magnitude;
            }
            else
            {
                Reset();
            }

            unit.transform.position += offset;
        }
    }

    public override void Reset()
    {
        isActive = false;
        moveDest = unit.transform.position;
    }

    public Vector3 MoveDest
    {
        get { return moveDest; }
        set
        {
            if(value != moveDest && Utils.Approximately(value, unit.transform.position))
            {
                moveDest = value;
                Start();
            }
        }
    }
}
