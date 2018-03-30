using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ModelSpawner : LoadableBehaviour {

    [SerializeField] protected List<ModelSpawnData> modelSpawnData = new List<ModelSpawnData>();

    public event Action onSpawnEvent;

    protected override void LoadDataInternal()
    {
        if (!Application.isPlaying) return;

        SpawnAll();
    }

    protected override void UnloadDataInternal()
    {
        DespawnAll();
    }

    protected abstract GameObject GetModel(int id = 0);

    public void SpawnAll()
    {
        for (int i = 0; i < modelSpawnData.Count; i++)
        {
            Spawn(i);
        }
    }

    private void Spawn(int modelId)
    {

        ModelSpawnData msData = modelSpawnData[modelId];
        if (msData.spawnedObject != null)
        {
            if (msData.shouldRespawn)
            {
                Despawn(modelId);
            }
            else
                return;
        }

        GameObject modelObject = GetModel(modelId);
        if (modelObject == null)
        {
            return;
        }

        msData.spawnedObject = modelObject;
        msData.spawnedObject.transform.localPosition += msData.localPositionOffset;
        msData.spawnedObject.transform.Rotate(msData.localRotationOffset, Space.Self);

        msData.shouldRespawn = false;
        onSpawnEvent?.Invoke();
    }

    private void Despawn(int id = 0)
    {
        Destroy(modelSpawnData[id].spawnedObject);
        modelSpawnData[id].spawnedObject.tag = "Untagged";
    }

    private void DespawnAll()
    {
        for (int i = 0; i < modelSpawnData.Count; i++)
        {
            Despawn(i);
        }
    }

    public void ShouldRespawn()
    {
        foreach(var i in modelSpawnData)
        {
            i.shouldRespawn = true;
        }
        Debug.Log("I SHOULD RESPWAN");
        Debug.Log(MainData.Save.CurrentSlotID);
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
