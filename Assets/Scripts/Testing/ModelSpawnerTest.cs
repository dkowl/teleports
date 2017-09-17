using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSpawnerTest : MonoBehaviour {

    public string raceName;

    private GameObject spawnedObject;
    private SkinnedMeshRenderer meshRenderer;

    public void Awake()
    {
        spawnedObject = new GameObject(raceName + "model");
        spawnedObject.transform.parent = transform;
        spawnedObject.AddComponent<SkinnedMeshRenderer>();
        meshRenderer = spawnedObject.GetComponent<SkinnedMeshRenderer>(); 
        meshRenderer = MainData.CurrentGameData.GetRace("raceName").Mesh;

    }
}
