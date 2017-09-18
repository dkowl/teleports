using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LoadableBehaviour : MonoBehaviour {

	void OnEnable()
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

    private void Unsubscribe()
    {
        MainData.OnInitializedEvent -= LoadData;
        SaveData.OnCharacterIDChangedEvent -= LoadData;
    }

    public void LoadData()
    {
        Unsubscribe();
        MainData.OnInitializedEvent += LoadData;
        SaveData.OnCharacterIDChangedEvent += LoadData;

        if (MainData.instance == null)
        {
            return;
        }
        else
        {
            LoadDataInternal();
        }
    }

    public abstract void LoadDataInternal();
}
