using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {

    public GameObject prefab;
    
    protected bool isSpawned = false;    
    protected GameObject spawnedInstance;

    public void OnEnable()
    {
        Spawn();
    }

    public void OnDisable()
    {
        Despawn();
    }

    public void OnDestroy()
    {
        Despawn();
    }

    public void Spawn()
    {
        if (prefab != null)
        {
            if (!isSpawned || spawnedInstance == null)
            {
                BeforeSpawn();
                spawnedInstance = Instantiate(prefab, transform);
                spawnedInstance.hideFlags = HideFlags.DontSave;
                AfterSpawn();
            }
            isSpawned = true;
        }
    }

    public virtual void BeforeSpawn()
    {

    }

    public virtual void AfterSpawn()
    {

    }

    public void Despawn()
    {
        if (isSpawned)
        {
            DestroyImmediate(spawnedInstance);
        }
        isSpawned = false;
    }
}
