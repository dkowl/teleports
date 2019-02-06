using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Teleports.Utils;

[ExecuteInEditMode]
public class PrefabSpawner : SerializedMonoBehaviour {

    [SerializeField] protected GameObject prefab;
    [SerializeField] protected int spawnAmount = 1;
    [SerializeField] private Transform spawnTransform;
    
    protected List<bool> isSpawned;
    protected List<GameObject> spawnedInstances;
    protected int currentId;

    private bool isInitialized = false;
    private System.Action afterSpawnAction;

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

    // init spawn amount and prefab here
    protected virtual void PreInitialize() { }

    protected virtual void PostInitialize() { }

    protected virtual void BeforeSpawn() { }

    protected virtual void AfterSpawn()
    {
        afterSpawnAction?.Invoke();
    }

    public void Initialize()
    {
        if (isInitialized && spawnAmount == spawnedInstances.Count) return;

        PreInitialize();
        if (spawnedInstances != null && spawnedInstances.Count > 0)
            Despawn();
        Utils.InitWithValues(ref isSpawned, spawnAmount, false);
        Utils.InitWithValues(ref spawnedInstances, spawnAmount, null);
        PostInitialize();
        isInitialized = true;
    }

    public void Spawn()
    {
        Initialize();
        if (prefab == null) return;
        for (currentId = 0; currentId < spawnAmount; currentId++)
        {
            if (isSpawned[currentId] && spawnedInstances[currentId] != null) continue;

            if (!isSpawned[currentId] && spawnedInstances[currentId] != null)
            {
                Debug.LogWarning("Prefab marked as not spawned even though it is spawned");
                DestroyImmediate(spawnedInstances[currentId]);
            }

            BeforeSpawn();
            spawnedInstances[currentId] = Instantiate(prefab, SpawnTransform);
            spawnedInstances[currentId].hideFlags = HideFlags.DontSave;
            isSpawned[currentId] = true;
            AfterSpawn();

        }
    }

    public void Despawn()
    {
        Debug.Assert(isInitialized);
        for (currentId = 0; currentId < spawnedInstances.Count; currentId++)
        {
            if (!isSpawned[currentId]) continue;

            DestroyImmediate(spawnedInstances[currentId]);
            isSpawned[currentId] = false;
        }
    }

    public void Respawn()
    {
        Initialize();
        Despawn();
        Spawn();
    }

    protected GameObject SpawnedInstance => spawnedInstances[currentId];
    protected int CurrentInstanceId => currentId;

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
            Spawn();
        }
    }

    public Transform SpawnTransform
    {
        get { return spawnTransform == null ? transform : spawnTransform; }
        set { spawnTransform = value; }
    }

    public System.Action AfterSpawnAction
    {
        set { afterSpawnAction = value; }
    }

    public bool ArePrefabsSpawned
    {
        get
        {
            foreach (var i in isSpawned)
            {
                if (!i) return false;
            }

            return true;
        }
    }
}
