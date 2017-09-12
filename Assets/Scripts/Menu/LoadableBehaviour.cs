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
        MainData.OnInitializedEvent -= LoadData;
    }

    public void LoadData()
    {
        MainData.OnInitializedEvent += LoadData;

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
