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

    abstract protected void LoadDataInternal();

    virtual protected void SubscribeInternal()
    {
        MainData.OnInitializedEvent += LoadData;
        SaveData.OnCharacterIDChangedEvent += LoadData;
        Menu.OnShowEvent += LoadData;
        Menu.OnHideEvent += LoadData;
    }

    virtual protected void UnsubscribeInternal()
    {
        MainData.OnInitializedEvent -= LoadData;
        SaveData.OnCharacterIDChangedEvent -= LoadData;
        Menu.OnShowEvent -= LoadData;
        Menu.OnHideEvent -= LoadData;
    }

    public void LoadData()
    {
        if (gameObject.activeSelf)
        {
            Subscribe();
            LoadDataInternal();
        }
    }

    private void Subscribe()
    {
        if (!isSubscribed)
        {
            Unsubscribe();
            SubscribeInternal();
            isSubscribed = true;
        }
    }

    private void Unsubscribe()
    {
        if (isSubscribed)
        {
            UnsubscribeInternal();
            isSubscribed = false;
        }
    }
}
