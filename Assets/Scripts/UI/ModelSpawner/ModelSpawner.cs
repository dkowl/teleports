using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ModelSpawner : LoadableBehaviour {

    [Serializable]
    public class ModelSpawnData
    {
        [SerializeField] public Vector3 localPositionOffset;
        [SerializeField] public Vector3 localRotationOffset;

        [NonSerialized] public GameObject spawnedObject;
        [NonSerialized] public bool shouldRespawn;
    }

    [SerializeField] protected List<ModelSpawnData> modelSpawnData;

    public override void LoadDataInternal()
    {
        if (Application.isPlaying)
        {
            for(int i = 0; i<modelSpawnData.Count; i++)
            {
                ModelSpawnData msData = modelSpawnData[i];
                if (msData.spawnedObject != null && msData.shouldRespawn)
                {
                    if (msData.shouldRespawn)
                    {
                        Destroy(msData.spawnedObject);
                        msData.spawnedObject.tag = "Untagged";
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
            }
        }
    }

    protected abstract GameObject GetModel(int id = 0);
}
