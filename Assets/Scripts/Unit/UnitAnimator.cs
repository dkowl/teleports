using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimator : MonoBehaviour {

    private Unit unit;
    private Animator animator;

    private int moveSpeedHash;
    private int isMovingHash;
    private int castHash, castResetHash;
    private int castStartHash01, castStartHash02; //there are two of these to handle transitioning from one skill casted immediately after another, so both clips can exist in the state machine simultaneously
    private int castSpeedHash;
    private bool cast02Flag;
    private List<int> triggers;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        animator = GetComponent<Animator>();

        moveSpeedHash = Animator.StringToHash("moveSpeed");
        isMovingHash = Animator.StringToHash("isMoving");
        castHash = Animator.StringToHash("cast");
        castStartHash01 = Animator.StringToHash("castStart01");
        castStartHash02 = Animator.StringToHash("castStart02");
        castResetHash = Animator.StringToHash("castReset");
        castSpeedHash = Animator.StringToHash("castSpeed");

        triggers = new List<int> { castHash, castResetHash, castStartHash01, castStartHash02 };
    }

    private void Start () {
        Subscribe();
	}

    private void Update () {
        animator.SetFloat(moveSpeedHash, unit.MoveSpeed);
        animator.SetBool(isMovingHash, unit.IsMoving);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void HandleCastStartEvent(CastingState.CastEventArgs eventArgs)
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

        Debug.LogFormat("Combo counter: {0}", eventArgs.Skill.ComboCounter);
        if (eventArgs.Skill.ComboCounter > 0) return;

        if (eventArgs.Skill.Graphics != null)
        {
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;   
            overrideController[clipName] = eventArgs.Skill.Graphics.CastAnimation;
            //animator.runtimeAnimatorController = overrideController;
        }
        animator.SetFloat(castSpeedHash, eventArgs.Skill.GetSpeedModifier(unit));
        SetTrigger(castStartHash);
    }

    private void HandleCastEvent(CastingState.CastEventArgs eventArgs)
    {
        SetTrigger(castHash);
    }

    private void HandleCastResetEvent(CastingState.CastEventArgs eventArgs)
    {
        Debug.Log("Triggering castReset");
        SetTrigger(castResetHash);
    }

    private void SetTrigger(int trigger)
    {
        foreach(var t in triggers)
            if (t != trigger) animator.ResetTrigger(t);
        animator.SetTrigger(trigger);
    }

    private void Subscribe()
    {
        Unsubscribe();
        unit.CastingState.startCastEvent += HandleCastStartEvent;
        unit.CastingState.castEvent += HandleCastEvent;
        unit.CastingState.resetCastEvent += HandleCastResetEvent;
    }

    private void Unsubscribe()
    {
        unit.CastingState.startCastEvent -= HandleCastStartEvent;
        unit.CastingState.castEvent -= HandleCastEvent;
        unit.CastingState.resetCastEvent -= HandleCastResetEvent;
    }
}
