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
            Vector3 offset = moveDest - unit.Rigidbody.position;
            Vector3 targetVelocity = offset.normalized * unit.MoveSpeed;

            if (unit.MoveSpeed * dTime < offset.magnitude)
            {
                offset *= unit.MoveSpeed * dTime / offset.magnitude;
            }
            else
            {
                Reset();
            }
            
            unit.Rigidbody.velocity = targetVelocity;
            //unit.Rigidbody.velocity = CalculateVelocity(unit.Rigidbody.velocity, targetVelocity);
            //unit.Rigidbody.AddForce(CalculateForce(unit.Rigidbody.velocity, targetVelocity), ForceMode.Acceleration);
            //unit.Rigidbody.MovePosition(unit.Rigidbody.position + offset);
        }
        else if(IsActive && IsBlocked)
        {
            Reset();
        }
    }

    public override void Reset()
    {
        isActive = false;
        moveDest = unit.Rigidbody.position;
        //unit.Rigidbody.velocity = Vector3.zero;
    }

    public void Start(Vector3 newMoveDest)
    {
        if (newMoveDest != moveDest && !Utils.Approximately(newMoveDest, unit.transform.position))
        {
            moveDest = newMoveDest;
            Start();
        }
    }

    Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 targetVelocity)
    {
        float currentMagnitude = Vector3.Dot(currentVelocity, targetVelocity.normalized);
        float magnitudeDelta = targetVelocity.magnitude - currentMagnitude;
        
        return currentVelocity + targetVelocity.normalized * magnitudeDelta;
    }

    public Vector3 MoveDest
    {
        get { return moveDest; }
    }
}
