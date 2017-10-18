using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected CommandState state;
    public delegate void FinishCallback();
    protected FinishCallback finishCallback;

    public void Start()
    {
        state = CommandState.InProgress;
        StartInternal();
    }

    public void Finish()
    {
        FinishInternal();
        state = CommandState.Finished;
        if (finishCallback != null)
        {
            finishCallback();
        }
    }

    public void RegisterFinishCallback(FinishCallback callback)
    {
        finishCallback = callback;
    }

    protected abstract void StartInternal();

    protected abstract void FinishInternal();

    public CommandState State
    {
        get { return state; }
        private set { state = value; }
    }
}
