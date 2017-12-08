using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {

    [SerializeField] protected GameObject prefab;
    [SerializeField] protected int spawnAmount = 1;
    
    protected List<bool> isSpawned;
    protected List<GameObject> spawnedInstances;
    protected int currentId;

    private bool isInitialized = false;

    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable()
    {
        Despawn();
    }

    private void OnDestroy()
    {
        Despawn();
    }

    protected virtual void OnInitialize() { }

    protected virtual void BeforeSpawn() { }
    
    protected virtual void AfterSpawn() { }

    public void Initialize()
    {
        if (!isInitialized || spawnAmount != spawnedInstances.Count)
        {
            if (spawnedInstances != null && spawnedInstances.Count > 0)
                Despawn();
            Utils.InitWithValues(ref isSpawned, spawnAmount, false);
            Utils.InitWithValues(ref spawnedInstances, spawnAmount, null);
            OnInitialize();
            isInitialized = true;
        }
    }

    public void Spawn()
    {
        Initialize();
        if (prefab != null)
        {
            for (currentId = 0; currentId < spawnAmount; currentId++)
            {
                if (!isSpawned[currentId] || spawnedInstances[currentId] == null)
                {
                    BeforeSpawn();
                    spawnedInstances[currentId] = Instantiate(prefab, transform);
                    spawnedInstances[currentId].hideFlags = HideFlags.DontSave;
                    AfterSpawn();
                }
                isSpawned[currentId] = true;

            }
        }
    }

    public void Despawn()
    {
        for (currentId = 0; currentId < spawnedInstances.Count; currentId++)
        {
            if (isSpawned[currentId])
            {
                DestroyImmediate(spawnedInstances[currentId]);
            }
            isSpawned[currentId] = false;
        }
    }

    public void Respawn()
    {
        Despawn();
        Spawn();
    }

    public GameObject SpawnedInstance
    {
        get { return spawnedInstances[currentId]; }
    }

    public GameObject Prefab
    {
        set { prefab = value; }
    }

    public int SpawnAmount
    {
        set {
            Despawn();
            spawnAmount = value;
            isInitialized = false;
            Initialize();
            Spawn();
        }
    }
}
