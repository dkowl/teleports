using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingState : ActionState {

    private CastCommand currentCommand;
    private CastCommand lastCommand;
    private float currentCastTime;
    private bool hasCasted;
    private int comboCounter;

    

    public event Action<CastEventArgs> startCastEvent, castEvent, resetCastEvent;

    public CastingState(Unit unit) : base(unit)
    {
        Reset();
    }

    protected override void OnStart()
    {
        /*if(Unit.name == "Player") Debug.Log(
            "castTarget: " + TargetInfo.TargetUnit.name +
            " ||| activeSkill: " + ActiveSkill.UniqueName + 
            " ||| combo: " + comboCounter.ToString());*/
        Debug.Assert(currentCommand != null && currentCommand.IsValid());
        if (lastCommand != null && lastCommand.Skill == currentCommand.Skill) comboCounter++;
        lastCommand = currentCommand;
        currentCastTime = 0;
        hasCasted = false;
        if (startCastEvent != null) startCastEvent(new CastEventArgs(this));
    }

    protected override void OnUpdate(float dTime)
    {
        currentCastTime += dTime;
        if (currentCastTime >= ActiveSkill.CastTime && !hasCasted)
        {
            ActiveSkill.Cast(Unit, TargetInfo);
            if (castEvent != null) castEvent(new CastEventArgs(this));
            hasCasted = true;
        }
        if(currentCastTime >= ActiveSkill.TotalCastTime)
        {
            currentCommand = null;
            if (!lastCommand.IsInterrupt)
            {
                Start(lastCommand);
            }
            else
            {
                Reset();
            }
        }
    }

    protected override void OnReset()
    {
        currentCommand = null;
        lastCommand = null;
        currentCastTime = 0;
        hasCasted = false;
        comboCounter = 0;
        if (resetCastEvent != null) resetCastEvent(new CastEventArgs(this));
        if(Unit.name == "Player") Debug.Log("Resetting" + Unit.name);
    }

    public TryStartResult TryStart(CastCommand command)
    {
        if (command == null) return TryStartResult.None;
        switch (GetState())
        {
            case State.Interruptable:
                return Start(command);
            case State.NonInterruptable:
                lastCommand = command;
                return TryStartResult.Save;
            default:
                return TryStartResult.None;
        }
    }

    public TryStartResult TryStart(Skill skill, Skill.TargetInfo targetInfo)
    {
        return TryStart(new CastCommand(skill, targetInfo));
    }

    public TryStartResult TryInterrupt()
    {
        return TryStart(new CastCommand());
    }

    private TryStartResult Start(CastCommand command)
    {
        if (!command.Equals(currentCommand))
        {
            if (command.IsValid())
            {
                currentCommand = command;
                Start();
                return TryStartResult.Start;
            }
            else
            {
                Reset();
                return TryStartResult.Reset;
            }
        }
        return TryStartResult.None;
    }

    Skill.TargetInfo TargetInfo
    {
        get {
            if (currentCommand != null) return currentCommand.TargetInfo;
            else return null;
        }
    }

    Skill ActiveSkill
    {
        get {
            if (currentCommand != null) return currentCommand.Skill;
            else return null;
        }
    }

    public State GetState()
    {
        if(ActiveSkill == null || currentCastTime < ActiveSkill.EarlyBreakTime)
        {
            return State.Interruptable;
        }
        else
        {
            return State.NonInterruptable;
        }
    }

    public enum State
    {
        Interruptable,
        NonInterruptable
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

    public class CastCommand
    {
        Type type;
        Skill skill;
        Skill.TargetInfo targetInfo;

        public CastCommand(Skill skill, Skill.TargetInfo targetInfo)
        {
            type = Type.Cast;
            this.skill = skill;
            this.targetInfo = new Skill.TargetInfo(targetInfo);
        }

        public CastCommand()
        {
            type = Type.Interrupt;
            skill = null;
            targetInfo = null;
        }

        public bool IsValid()
        {
            if (type == Type.Interrupt) return false;
            else if (skill == null || targetInfo == null) return false;
            else if (!skill.CanReachTarget(targetInfo)) return false;
            else return true;
        }

        public override bool Equals(object obj)
        {
            CastCommand other = (CastCommand)obj;
            if (other == null) return false;
            else
            {
                return
                    type == other.type &&
                    skill == other.skill &&
                    targetInfo.Equals(other.targetInfo);
            }
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ skill.GetHashCode() ^ targetInfo.GetHashCode();
        }

        public bool IsInterrupt
        {
            get { return type == Type.Interrupt; }
        }

        public Skill Skill
        {
            get { return skill; }
        }

        public Skill.TargetInfo TargetInfo
        {
            get { return targetInfo; }
        }

        public enum Type
        {
            Cast,
            Interrupt
        }
    }

    public enum TryStartResult
    {
        None,
        Reset,
        Start,
        Save
    }


}
