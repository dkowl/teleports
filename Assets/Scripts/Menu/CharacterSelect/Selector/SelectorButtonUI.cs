using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectorButtonUI : LoadableBehaviour
{

    public float inactiveScaleFactor = 0.8f;

    public override void LoadDataInternal()
    {
        if (IsActive())
        {
            SetActive();
        }
        else
        {
            SetInactive();
        }
    }

    protected abstract bool IsActive();
    protected abstract void OnActivate();
    protected abstract void OnDeactivate();

    public void SetActive()
    {
        transform.localScale = Vector3.one;
        if (!IsActive())
        {
            OnActivate();
        }
    }

    public void SetInactive()
    {
        transform.localScale = Vector3.one * inactiveScaleFactor;
    }
}