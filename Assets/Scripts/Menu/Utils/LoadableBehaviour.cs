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
        Unsubscribe();
    }

    protected abstract void LoadDataInternal();

    protected virtual void SubscribeInternal()
    {
        MainData.OnInitializedEvent += LoadData;
        SaveData.OnCharacterIDChangedEvent += LoadData;
        Menu.OnShowEvent += LoadData;
        Menu.OnHideEvent += LoadData;
    }

    protected virtual void UnsubscribeInternal()
    {
        MainData.OnInitializedEvent -= LoadData;
        SaveData.OnCharacterIDChangedEvent -= LoadData;
        Menu.OnShowEvent -= LoadData;
        Menu.OnHideEvent -= LoadData;
    }

    public void LoadData()
    {
        if (!gameObject.activeSelf) return;

        Subscribe();
        LoadDataInternal();
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
