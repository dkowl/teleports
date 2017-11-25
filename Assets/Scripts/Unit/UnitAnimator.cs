using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimator : MonoBehaviour {

    Unit unit;
    Animator animator;

    int moveSpeedHash;
    int isMovingHash;
    int castHash, castStartHash, castResetHash;

    void Awake()
    {
        unit = GetComponentInParent<Unit>();
        animator = GetComponent<Animator>();

        moveSpeedHash = Animator.StringToHash("moveSpeed");
        isMovingHash = Animator.StringToHash("isMoving");
        castHash = Animator.StringToHash("cast");
        castStartHash = Animator.StringToHash("castStart");
        castResetHash = Animator.StringToHash("castReset");
    }
    
	void Start () {
        unit.CastingState.startCastEvent += HandleCastStartEvent;
        unit.CastingState.castEvent += HandleCastEvent;
        unit.CastingState.resetCastEvent += HandleCastResetEvent;
	}
	
	void Update () {
        animator.SetFloat(moveSpeedHash, unit.MoveSpeed);
        animator.SetBool(isMovingHash, unit.IsMoving);
	}

    void HandleCastStartEvent(object sender, EventArgs e)
    {
        animator.SetTrigger(castStartHash);
    }

    void HandleCastEvent(object sender, EventArgs e)
    {
        animator.SetTrigger(castHash);
    }

    void HandleCastResetEvent(object sender, EventArgs e)
    {
        animator.SetTrigger(castResetHash);
    }
}
