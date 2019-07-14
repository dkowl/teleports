using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LoadableBehaviour : MonoBehaviour {

    private bool isSubscribed = false;

    private void OnEnable()
    {
        LoadData();
    }

    private void OnDestroy()
    {
        UnloadData();
    }

    protected abstract void LoadDataInternal();
    protected virtual void UnloadDataInternal() { }

    protected virtual void SubscribeInternal()
    {
        Main.AfterInitializationEvent += LoadData;
        GameState.HeroChangedEvent += LoadData;
        GameState.GameStateUpdatedEvent += LoadData;
        Menu.OnShowEvent += LoadData;
        Menu.OnHideEvent += LoadData;
    }

    protected virtual void UnsubscribeInternal()
    {
        Main.AfterInitializationEvent -= LoadData;
        GameState.HeroChangedEvent -= LoadData;
        GameState.GameStateUpdatedEvent -= LoadData;
        Menu.OnShowEvent -= LoadData;
        Menu.OnHideEvent -= LoadData;
    }

    public void LoadData()
    {
        if (!gameObject.activeSelf) return;

        Subscribe();
        LoadDataInternal();
    }

    public void UnloadData()
    {
        Unsubscribe();
        UnloadDataInternal();
    }

    private void Subscribe()
    {
        if (isSubscribed) return;

        Unsubscribe();
        SubscribeInternal();
        isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (!isSubscribed) return;

        UnsubscribeInternal();
        isSubscribed = false;
    }
}
