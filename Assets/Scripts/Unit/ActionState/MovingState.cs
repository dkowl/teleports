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

    protected override void OnStart()
    {
        Unit.RotatingState.RotationTarget = Quaternion.LookRotation(moveDest - Unit.transform.position);
    }

    protected override void OnUpdate(float dTime)
    {
        if (Unit.MoveSpeed > 0)
        {
            Vector3 offset = moveDest - Unit.Physics.Rigidbody.position;
            Vector3 targetVelocity = offset.normalized * Unit.MoveSpeed;

            Unit.Physics.Rigidbody.velocity = targetVelocity;
            //unit.Rigidbody.velocity = CalculateVelocity(unit.Rigidbody.velocity, targetVelocity);
            //unit.Rigidbody.AddForce(CalculateForce(unit.Rigidbody.velocity, targetVelocity), ForceMode.Acceleration);
            //unit.Rigidbody.MovePosition(unit.Rigidbody.position + offset);
        }
        if ((moveDest - Unit.transform.position).magnitude < 10 * dTime)
        {
            Reset();
        }
    }

    protected override void OnReset()
    {
        moveDest = Unit.Physics.Rigidbody.position;
        //unit.Rigidbody.velocity = Vector3.zero;
    }

    public void Start(Vector3 newMoveDest)
    {
        if (!Utils.Approximately(newMoveDest, Unit.transform.position))
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
