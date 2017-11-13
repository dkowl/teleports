using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LoadableBehaviour : MonoBehaviour {

    private bool isSubscribed = false;

	virtual protected void OnEnable()
    {        
        LoadData();
    }

    virtual protected void OnDestroy()
    {
        Unsubscribe();
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
        if (gameObject.activeSelf)
        {
            Subscribe();
            LoadDataInternal();
        }
    }

    public abstract void LoadDataInternal();
}
