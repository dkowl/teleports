using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {

    public GameObject prefab;

    [SerializeField]
    [HideInInspector]
    protected bool isSpawned = false;

    [SerializeField]
    [HideInInspector]
    protected GameObject spawnedInstance;

    public void OnEnable()
    {
        Spawn();
        AfterSpawn();
    }

    public void OnDisable()
    {
        Despawn();
    }

    public void Spawn()
    {
        if (!isSpawned)
        {
            spawnedInstance = Instantiate(prefab, transform);
        }
        isSpawned = true;
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
