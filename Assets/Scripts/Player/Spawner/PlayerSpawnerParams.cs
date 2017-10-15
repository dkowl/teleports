using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerParams {

    public enum SpawnType { World, UI };

    private GameObject parentObject;
    private SpawnType type;

    public PlayerSpawnerParams(GameObject parentObject, SpawnType type)
    {
        this.parentObject = parentObject;
        this.type = type;
    }

    public GameObject ParentObject
    {
        get { return parentObject; }
    }

    public SpawnType Type
    {
        get { return type; }
    }
}
