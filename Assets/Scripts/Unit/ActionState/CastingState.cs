using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingState : ActionState {

    protected Skill.TargetInfo targetInfo;
    protected Skill activeSkill;
    protected float currentCastTime;
    protected float currentLockTime;
    protected State state;

    public event Action<CastEventArgs> startCastEvent, castEvent, resetCastEvent;

    public CastingState(Unit unit) : base(unit)
    {
        Reset();
    }

    public override void Start()
    {
        /*Debug.Log(
            "castTarget: " + CastTarget.TargetUnit.name +
            " ||| activeSkill: " + activeSkill.Name +
            " ||| current cooldown: " + activeSkill.CurrentCooldown.ToString() +
            " ||| can reach cast target?: " + CanReachCastTarget.ToString());*/
        if(!IsActive && targetInfo != null && activeSkill != null && activeSkill.CurrentCooldown == 0 && CanReachTarget)
        {
            currentCastTime = 0;
            isActive = true;
            state = State.BeforeCast;
            if (startCastEvent != null) startCastEvent(new CastEventArgs(this));
        }
    }

    public override void Update(float dTime)
    {
        if (isActive && !IsBlocked)
        {
            if (state == State.BeforeCast)
            {
                currentCastTime += dTime;
                if (currentCastTime >= activeSkill.CastTime)
                {
                    activeSkill.Cast(unit, targetInfo);
                    if (castEvent != null) castEvent(new CastEventArgs(this));
                    state = State.AfterCast;
                }
            }
            else if(state == State.AfterCast)
            {
                currentLockTime += dTime;
                if (currentLockTime >= activeSkill.AfterCastLockTime)
                    Reset();
            }
        }
        else if(isActive)
        {
            Reset();
        }
    }

    public override void Reset()
    { 
        targetInfo = null;
        activeSkill = null;
        currentCastTime = 0;
        currentLockTime = 0;
        isActive = false;
        state = State.Ready;
        if (resetCastEvent != null) resetCastEvent(new CastEventArgs(this));
    }

    public void Start(Skill skill, Skill.TargetInfo target, bool interrupt = false)
    {
        if (IsActive)
        {
            if (interrupt)
                Reset();
            else
                return;
        }
        activeSkill = skill;
        targetInfo = target;
        Start();
    }

    bool CanReachTarget
    {
        get
        {
            return activeSkill.CanReachTarget(targetInfo);
        }
    }

    Skill.TargetInfo TargetInfo
    {
        get { return targetInfo; }
    }

    Skill ActiveSkill
    {
        get { return activeSkill; }
    }

    public enum State
    {
        BeforeCast,
        AfterCast,
        Ready
    }

    public class CastEventArgs
    {
        Skill skill;
        Skill.TargetInfo targetInfo;

        public CastEventArgs(Skill skill, Skill.TargetInfo targetInfo)
        {
            this.skill = skill;
            this.targetInfo = targetInfo;
        }

        public CastEventArgs(CastingState castingState)
        {
            skill = castingState.ActiveSkill;
            targetInfo = castingState.TargetInfo;
        }

        public Skill Skill
        {
            get { return skill; }
        }

        public Skill.TargetInfo TargetInfo
        {
            get { return targetInfo; }
        }
    }

}
