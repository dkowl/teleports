using System;
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

    protected Animator animator;
    protected bool[] hasParameter;
    protected CommandQueue commandQ = new CommandQueue();

    public delegate void CommandFinish();
    public event CommandFinish OpenFinishEvent, CloseFinishEvent, LoadFinishEvent;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        hasParameter = new bool[Utils.EnumCount(typeof(State))];
        string[] enumNames = Enum.GetNames(typeof(State));

        if (animator == null) return;
        for (int i = 0; i < enumNames.Length; i++)
        {
            hasParameter[i] = animator.HasParameter(enumNames[i]);
        }
    }

    protected virtual void OnDisable()
    {
        Skip();
    }

    protected override void LoadDataInternal()
    {
        //print("Adding Load Command");
        AddCommand(MenuBehaviourCommand.Type.Load);
    }

    public void AddCommand(MenuBehaviourCommand.Type type)
    {
        DisplayCommandQueue();
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
        //print("Calling OnLoad");
        if (DetectChange())
        {
            CurrentState = State.Loading;
            OnLoadInternal();
        }
        else
        {
            LoadFinish();
        }
    }

    public void OpenFinish()
    {
        CurrentState = State.Open;
        OpenFinishEvent?.Invoke();
    }

    public void CloseFinish()
    {
        CurrentState = State.Closed;
        CloseFinishEvent?.Invoke();
    }

    public void LoadFinish()
    {
        //Debug.Log("Calling load finish");
        if(CurrentState == State.Loading) CurrentState = previousState;
        LoadFinishEvent?.Invoke();
    }

    public void Skip()
    {
        if (animator != null && animator.HasParameter("Skip"))
        {
            animator.SetTrigger("Skip");
        }
        //Debug.Log("CurrentState while skipping: " + CurrentState.ToString());
        switch (CurrentState)
        {
            case State.Closing:
                CloseFinish();
                break;
            case State.Loading:
                LoadFinish();
                break;
            case State.Opening:
                OpenFinish();
                break;
        }
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

    protected virtual bool DetectChange()
    {
        return true;
    }

    public State CurrentState
    {
        get { return state; }
        protected set
        {
            if (state == value) return;
            previousState = state;
            state = value;
            //Debug.Log("Changing state of " + name + " (" + previousState.ToString() + " ===> " + state.ToString() + ")");
            if (hasParameter[(int)state])
            {
                //Debug.Log("Triggering " + state.ToString());
                animator.SetTrigger(state.ToString());
            }
        }
    }

    //Debug methods
    private void DisplayCommandQueue()
    {
        commandQ.DebugPrintQueue();
    }
}
