using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LoadableBehaviour : MonoBehaviour {

	virtual protected void OnEnable()
    {        
        LoadData();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    protected void Subscribe()
    {
        Unsubscribe();
        MainData.OnInitializedEvent += LoadData;
        SaveData.OnCharacterIDChangedEvent += LoadData;
        Menu.OnShowEvent += LoadData;
        Menu.OnHideEvent += LoadData;
    }

    protected void Unsubscribe()
    {
        MainData.OnInitializedEvent -= LoadData;
        SaveData.OnCharacterIDChangedEvent -= LoadData;
        Menu.OnShowEvent -= LoadData;
        Menu.OnHideEvent -= LoadData;
    }

    public void LoadData()
    {
        Subscribe();
        LoadDataInternal();        
    }

    public abstract void LoadDataInternal();
}
