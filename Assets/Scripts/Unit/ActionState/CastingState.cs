using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingState : ActionState {

    protected Skill.TargetInfo castTarget;
    protected Skill activeSkill;
    protected float currentCastTime;

    public event EventHandler startCastEvent, castEvent, resetCastEvent;

    public CastingState(Unit unit) : base(unit)
    {
        Reset();
    }

    public override void Start()
    {
        if(castTarget != null && activeSkill != null && activeSkill.CurrentCooldown == 0 && CanReachCastTarget)
        {
            currentCastTime = 0;
            isActive = true;
            if (startCastEvent != null) startCastEvent(this, EventArgs.Empty);
        }
    }

    public override void Update(float dTime)
    {
        if (isActive && !IsBlocked && CanReachCastTarget)
        {
            currentCastTime += dTime;
            if (currentCastTime >= activeSkill.CastTime)
            {
                activeSkill.Cast(unit, castTarget);
                if (castEvent != null) castEvent(this, EventArgs.Empty);
                Reset();
            }
        }
        else
        {
            Reset();
        }
    }

    public override void Reset()
    {
        castTarget = null;
        activeSkill = null;
        currentCastTime = 0;
        isActive = false;
        if (resetCastEvent != null) resetCastEvent(this, EventArgs.Empty);
    }

    public bool CanReachCastTarget
    {
        get
        {
            if (activeSkill != null && castTarget != null)
            {
                float totalReach = unit.Reach + unit.Size + activeSkill.Reach;
                if (castTarget.TargetUnit != null) totalReach += castTarget.TargetUnit.Size;

                return
                    (castTarget.Position - unit.transform.position).magnitude
                    <=
                    totalReach;
            }
            else
            {
                return false;
            }
        }
    }

    public Skill.TargetInfo CastTarget
    {
        get { return castTarget; }
        set
        {
            if (IsActive)
            {
                Reset();
            }
            castTarget = value;
        }
    }

    public Skill ActiveSkill
    {
        get { return activeSkill; }
        set
        {
            if (IsActive)
            {
                Reset();
            }
            activeSkill = value;
        }
    }

}
