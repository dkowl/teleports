using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState {

    private Unit unit;
    private bool isActive;
    private List<ActionState> blockers;

    public ActionState(Unit unit)
    {
        this.unit = unit;
        isActive = false;
        blockers = new List<ActionState>();
    }

    public void AddBlocker(ActionState blocker)
    {
        if (!blockers.Contains(blocker))
            blockers.Add(blocker);
    }

    protected void Start()
    {
        if (!IsBlocked)
        {
            isActive = true;
            OnStart();
        }
    }
    protected virtual void OnStart() { } 

    public void Update(float dTime)
    {
        if (IsActive)
        {
            if (!IsBlocked)
            {
                OnUpdate(dTime);
            }
            else
            {
                Reset();
            }
        }
    }
    protected virtual void OnUpdate(float dTime) { }

    protected void Reset()
    {
        if (IsActive)
        {
            isActive = false;
            OnReset();
        }
    }
    protected virtual void OnReset() { }

    public bool IsActive
    {
        get { return isActive; }
    }

    public bool IsBlocked
    {
        get
        {
            foreach(ActionState blocker in blockers)
            {
                if (blocker.IsActive)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public Unit Unit
    {
        get { return unit; }
    }
}
