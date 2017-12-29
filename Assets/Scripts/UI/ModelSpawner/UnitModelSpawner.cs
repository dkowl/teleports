using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModelSpawner : ModelSpawner {

    private UnitData unitData;

    protected override GameObject GetModel(int id = 0)
    {
        if (unitData == null) return null;

        return Instantiate(UnitModelAssembler.GetModel(unitData));
    }

    public UnitData UnitData
    {
        set
        {
            unitData = value;
            ShouldRespawn();
        }
    }

    public SkinnedMeshRenderer SkinnedMeshRenderer
    {
        get
        {
            if (Application.isPlaying)
            {
                return modelSpawnData[0].spawnedObject.GetComponentInChildren<SkinnedMeshRenderer>();
            }
            else
                return null;
        }
    }
}
