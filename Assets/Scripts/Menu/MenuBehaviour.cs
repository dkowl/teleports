using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

[RequireComponent(typeof(Animator))]
public abstract class MenuBehaviour : LoadableBehaviour {

    public enum State
    {
        Opening,
        Open,
        Closing,
        Closed,
        Loading
    }

    protected State state;

    Animator animator;
    protected bool[] hasParameter;

    protected void Awake()
    {
        animator = GetComponent<Animator>();

        hasParameter = new bool[Utils.EnumCount(typeof(State))];
        string[] enumNames = Enum.GetNames(typeof(State));
        for(int i = 0; i<enumNames.Length; i++)
        {
            hasParameter[i] = animator.HasParameter(enumNames[i]);
        }
    }

    public override void LoadDataInternal()
    {
        OnLoad();
    }

    public virtual void OnOpen()
    {
        if(state != State.Opening && state != State.Open)
        {
            CurrentState = State.Opening;
            OnOpenInternal();
        }
    }

    public virtual void OnClose()
    {
        if(state != State.Closing && state != State.Closed)
        {
            CurrentState = State.Closing;
            OnCloseInternal();
        }
    }

    public virtual void OnLoad()
    {
        CurrentState = State.Loading;
        OnLoadInternal();
    }

    protected virtual void OnOpenInternal() { }
    protected virtual void OnCloseInternal() { }
    protected virtual void OnLoadInternal() { }

    public State CurrentState
    {
        get { return state; }
        protected set
        {
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
