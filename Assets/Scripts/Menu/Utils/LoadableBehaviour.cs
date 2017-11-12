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

    protected virtual void Subscribe()
    {
        if (!isSubscribed)
        {
            Unsubscribe();
            MainData.OnInitializedEvent += LoadData;
            SaveData.OnCharacterIDChangedEvent += LoadData;
            Menu.OnShowEvent += LoadData;
            Menu.OnHideEvent += LoadData;
            SubscribeInternal();
            isSubscribed = true;
        }
    }

    protected virtual void Unsubscribe()
    {
        if (isSubscribed)
        {
            MainData.OnInitializedEvent -= LoadData;
            SaveData.OnCharacterIDChangedEvent -= LoadData;
            Menu.OnShowEvent -= LoadData;
            Menu.OnHideEvent -= LoadData;
            UnsubscribeInternal();
            isSubscribed = false;
        }
    }

    protected virtual void SubscribeInternal()
    {

    }

    protected virtual void UnsubscribeInternal()
    {

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
