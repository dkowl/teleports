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
        Subscribe();
	}
	
	void Update () {
        animator.SetFloat(moveSpeedHash, unit.MoveSpeed);
        animator.SetBool(isMovingHash, unit.IsMoving);
	}

    void HandleCastStartEvent(CastingState.CastEventArgs eventArgs)
    {
        animator.SetTrigger(castStartHash);
    }

    void HandleCastEvent(CastingState.CastEventArgs eventArgs)
    {
        animator.SetTrigger(castHash);
    }

    void HandleCastResetEvent(CastingState.CastEventArgs eventArgs)
    {
        animator.SetTrigger(castResetHash);
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    void Subscribe()
    {
        Unsubscribe();
        unit.CastingState.startCastEvent += HandleCastStartEvent;
        unit.CastingState.castEvent += HandleCastEvent;
        unit.CastingState.resetCastEvent += HandleCastResetEvent;
    }

    void Unsubscribe()
    {
        unit.CastingState.startCastEvent -= HandleCastStartEvent;
        unit.CastingState.castEvent -= HandleCastEvent;
        unit.CastingState.resetCastEvent -= HandleCastResetEvent;
    }
}
