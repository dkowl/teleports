using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ModelSpawner : LoadableBehaviour {

    [SerializeField] protected List<ModelSpawnData> modelSpawnData = new List<ModelSpawnData>();

    public event Action onSpawnEvent;

    override protected void LoadDataInternal()
    {
        if (Application.isPlaying)
        {
            for(int i = 0; i<modelSpawnData.Count; i++)
            {
                ModelSpawnData msData = modelSpawnData[i];
                if (msData.spawnedObject != null)
                {
                    if (msData.shouldRespawn)
                    {
                        Despawn(i);
                    }
                    else
                        continue;
                }

                GameObject modelObject = GetModel(i);

                if(modelObject == null)
                {
                    continue;
                }

                msData.spawnedObject = modelObject;
                msData.spawnedObject.transform.localPosition += msData.localPositionOffset;
                msData.spawnedObject.transform.Rotate(msData.localRotationOffset, Space.Self);

                msData.shouldRespawn = false;
                if(onSpawnEvent != null) onSpawnEvent();
            }
        }
    }

    protected abstract GameObject GetModel(int id = 0);

    private void Despawn(int id = 0)
    {
        Destroy(modelSpawnData[id].spawnedObject);
        modelSpawnData[id].spawnedObject.tag = "Untagged";
    }

    public void ShouldRespawn()
    {
        foreach(var i in modelSpawnData)
        {
            i.shouldRespawn = true;
        }
        LoadDataInternal();
    }

    public void AddSpawnData()
    {
        modelSpawnData.Add(new ModelSpawnData());
    }

    public void SetPositionOffset(Vector3 offset, int id = 0)
    {
        if(id < modelSpawnData.Count)
        {
            modelSpawnData[id].localPositionOffset = offset;
        }
    }

    public void SetRotationOffset(Vector3 offset, int id = 0)
    {
        if (id < modelSpawnData.Count)
        {
            modelSpawnData[id].localRotationOffset = offset;
        }
    }

    [Serializable]
    public class ModelSpawnData
    {
        [SerializeField] public Vector3 localPositionOffset;
        [SerializeField] public Vector3 localRotationOffset;

        [NonSerialized] public GameObject spawnedObject;
        [NonSerialized] public bool shouldRespawn;
    }
}
