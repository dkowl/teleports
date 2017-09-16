using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {

    public GameObject prefab;

    [SerializeField]
    protected bool isSpawned = false;

    [SerializeField]
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
                spawnedInstance = Instantiate(prefab, transform);
                spawnedInstance.hideFlags = HideFlags.DontSaveInEditor;
            }
            isSpawned = true;
        }
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
