using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState {

    private Unit unit;
    private bool isActive;
    private bool isPaused;
    private List<ActionState> blockers;
    private List<ActionState> pausers;

    public ActionState(Unit unit)
    {
        this.unit = unit;
        isActive = false;
        blockers = new List<ActionState>();
        pausers = new List<ActionState>();
    }

    public void AddBlocker(ActionState blocker)
    {
        if (!blockers.Contains(blocker))
            blockers.Add(blocker);
    }

    public void AddPauser(ActionState pauser)
    {
        if (!pausers.Contains(pauser))
            pausers.Add(pauser);
    }

    protected void Start()
    {
        if (!IsBlocked)
        {
            OnStart();
            isActive = true;
        }
    }
    protected virtual void OnStart() { } 

    public void Update(float dTime)
    {
        if (IsActive)
        {
            if (IsBlocked) Reset();
            else if (IsPaused) Pause();
            else if (isPaused && !IsPaused) Resume();
            else OnUpdate(dTime);
        }
    }
    protected virtual void OnUpdate(float dTime) { }

    private void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            OnPause();
        }
    }
    protected virtual void OnPause() { }

    private void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            OnResume();
        }
    }
    protected virtual void OnResume() { }

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

    public bool IsPaused
    {
        get
        {
            foreach (ActionState pauser in pausers)
            {
                if (pauser.IsActive) return true;
            }
            return false;
        }
    }

    public bool IsBlocked
    {
        get
        {
            foreach(ActionState blocker in blockers)
            {
                if (blocker.IsActive) return true;
            }
            return false;
        }
    }

    public Unit Unit
    {
        get { return unit; }
    }
}
