﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState {

    protected Unit unit;

    protected bool isActive = false;

    protected List<ActionState> blockers = new List<ActionState>();

    public ActionState(Unit unit)
    {
        this.unit = unit;
    }

    public abstract void Start();
    public abstract void Update(float dTime);
    public abstract void Reset();

    public void AddBlocker(ActionState blocker)
    {
        if(!blockers.Contains(blocker))
            blockers.Add(blocker);
    }

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