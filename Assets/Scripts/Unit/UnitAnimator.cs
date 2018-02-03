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
    int castHash, castResetHash;
    int castStartHash01, castStartHash02; //there are two of these to handle transitioning from one skill casted immediately after another, so both clips can exist in the state machine simultaneously
    bool cast02Flag;
    List<int> triggers;

    void Awake()
    {
        unit = GetComponentInParent<Unit>();
        animator = GetComponent<Animator>();

        moveSpeedHash = Animator.StringToHash("moveSpeed");
        isMovingHash = Animator.StringToHash("isMoving");
        castHash = Animator.StringToHash("cast");
        castStartHash01 = Animator.StringToHash("castStart01");
        castStartHash02 = Animator.StringToHash("castStart02");
        castResetHash = Animator.StringToHash("castReset");

        triggers = new List<int> { castHash, castResetHash, castStartHash01, castStartHash02 };
    }
    
	void Start () {
        Subscribe();
	}
	
	void Update () {
        animator.SetFloat(moveSpeedHash, unit.MoveSpeed);
        animator.SetBool(isMovingHash, unit.IsMoving);
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    void HandleCastStartEvent(CastingState.CastEventArgs eventArgs)
    {
        Debug.Log("Triggering castStart");

        string clipName;
        int castStartHash;
        if (cast02Flag)
        {
            clipName = "Cast02";
            castStartHash = castStartHash02;
        }
        else
        {
            clipName = "Cast01";
            castStartHash = castStartHash01;
        }
        cast02Flag = !cast02Flag;

        if (eventArgs.Skill.Graphics != null)
        {
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;   
            overrideController[clipName] = eventArgs.Skill.Graphics.CastAnimation;
            //animator.runtimeAnimatorController = overrideController;
        }
        SetTrigger(castStartHash);
    }

    void HandleCastEvent(CastingState.CastEventArgs eventArgs)
    {
        SetTrigger(castHash);
    }

    void HandleCastResetEvent(CastingState.CastEventArgs eventArgs)
    {
        Debug.Log("Triggering castReset");
        SetTrigger(castResetHash);
    }

    void SetTrigger(int trigger)
    {
        foreach(int t in triggers)
        {
            if (t != trigger) animator.ResetTrigger(t);
        }
        animator.SetTrigger(trigger);
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
