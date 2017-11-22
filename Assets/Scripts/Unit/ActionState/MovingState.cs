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

        unit.RotatingState.RotationTarget = Quaternion.LookRotation(moveDest - unit.transform.position);
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
        else if(IsActive && IsBlocked)
        {
            Reset();
        }
    }

    public override void Reset()
    {
        isActive = false;
        moveDest = unit.transform.position;
    }

    public void Start(Vector3 newMoveDest)
    {
        if (newMoveDest != moveDest && !Utils.Approximately(newMoveDest, unit.transform.position))
        {
            moveDest = newMoveDest;
            Start();
        }
    }

    public Vector3 MoveDest
    {
        get { return moveDest; }
    }
}
