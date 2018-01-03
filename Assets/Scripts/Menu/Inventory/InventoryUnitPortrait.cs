using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class InventoryUnitPortrait : PortraitUI {

    [SerializeField] UnitModelSpawner unitModelSpawner;

    protected override CameraMeshTargeter.MeshComponentType SpawnModel()
    {
        InventoryMenu parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);
        gameObject.InitComponent(ref unitModelSpawner);
        unitModelSpawner.AddSpawnData();
        unitModelSpawner.SetPositionOffset(SpecialSpawnPlaces.InventoryPlayer);
        unitModelSpawner.SetRotationOffset(new Vector3(0, 180, 0));
        unitModelSpawner.UnitData = parentMenu.UnitData;
        skinnedMeshRenderer = unitModelSpawner.SkinnedMeshRenderer;
        return CameraMeshTargeter.MeshComponentType.SkinnedMeshRenderer;
    }

}
