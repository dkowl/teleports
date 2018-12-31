using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected CommandState state = CommandState.Pending;
    public delegate void FinishCallback();
    protected List<FinishCallback> finishCallbacks = new List<FinishCallback>();

    public void Start()
    {
        state = CommandState.InProgress;
        StartInternal();
    }

    public void Finish()
    {
        if (state != CommandState.InProgress) return;

        FinishInternal();
        state = CommandState.Finished;
        foreach(var finishCallback in finishCallbacks)
        {
            finishCallback();
        }
        finishCallbacks.Clear();
    }

    public void RegisterFinishCallback(FinishCallback callback)
    {
        finishCallbacks.Add(callback);
    }

    protected abstract void StartInternal();

    protected abstract void FinishInternal();

    public CommandState State
    {
        get { return state; }
        private set { state = value; }
    }
}
