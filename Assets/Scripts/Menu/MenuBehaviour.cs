﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class MenuBehaviour : LoadableBehaviour {

    public enum State
    {
        Closed,
        Opening,
        Loading,
        Open,
        Closing
    }

    protected State state, previousState;

    Animator animator;
    protected bool[] hasParameter;
    protected CommandQueue commandQ = new CommandQueue();

    public delegate void CommandFinish();
    public event CommandFinish OpenFinishEvent, CloseFinishEvent, LoadFinishEvent;

    protected void Awake()
    {
        animator = GetComponent<Animator>();

        hasParameter = new bool[Utils.EnumCount(typeof(State))];
        string[] enumNames = Enum.GetNames(typeof(State));
        if (animator != null)
        {
            for (int i = 0; i < enumNames.Length; i++)
            {
                hasParameter[i] = animator.HasParameter(enumNames[i]);
            }
        }
    }

    public override void LoadDataInternal()
    {
        AddCommand(MenuBehaviourCommand.Type.Load);
    }

    public void AddCommand(MenuBehaviourCommand.Type type)
    {
        commandQ.AddCommand(new MenuBehaviourCommand(this, type));
    }

    public virtual void OnOpen()
    {
        if(state != State.Opening && state != State.Open)
        {
            CurrentState = State.Opening;
            OnOpenInternal();
        }
        else
        {
            OpenFinish();
        }
    }

    public virtual void OnClose()
    {
        if(state != State.Closing && state != State.Closed)
        {
            CurrentState = State.Closing;
            OnCloseInternal();
        }
        else
        {
            CloseFinish();
        }
    }

    public virtual void OnLoad()
    {
        CurrentState = State.Loading;
        OnLoadInternal();
    }

    public void OpenFinish()
    {
        CurrentState = State.Open;
        if (OpenFinishEvent != null) OpenFinishEvent();
    }

    public void CloseFinish()
    {
        CurrentState = State.Closed;
        if (CloseFinishEvent != null) CloseFinishEvent();
    }

    public void LoadFinish()
    {
        CurrentState = previousState;
        if (LoadFinishEvent != null) LoadFinishEvent();
    }

    protected virtual void OnOpenInternal() {
        if (!hasParameter[(int)State.Opening])
        {
            OpenFinish();
        }
    }
    protected virtual void OnCloseInternal() {

        if (!hasParameter[(int)State.Closing])
        {
            CloseFinish();
        }
    }
    protected virtual void OnLoadInternal() {

        if (!hasParameter[(int)State.Loading])
        {
            LoadFinish();
        }
    }

    public State CurrentState
    {
        get { return state; }
        protected set
        {
            if (state == value) return;
            previousState = state;
            state = value;
            Debug.Log("Changing state to " + state.ToString());
            if (hasParameter[(int)state])
            {
                Debug.Log("Triggering " + state.ToString());
                animator.SetTrigger(state.ToString());
            }
        }
    }
}