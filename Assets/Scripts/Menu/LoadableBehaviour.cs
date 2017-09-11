using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LoadableBehaviour : MonoBehaviour {

	void OnEnable()
    {        
        LoadData();
    }

    public void LoadData() {
        if (MainData.instance == null)
        {
            return;
        }
        else
        {
            Debug.Log("Loading from " + MainData.instance.name);
            LoadDataInternal();
        }
    }

    public abstract void LoadDataInternal();
}
