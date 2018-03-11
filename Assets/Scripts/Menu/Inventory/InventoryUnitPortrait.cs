using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class InventoryUnitPortrait : PortraitUI, IMessageHandler<ItemEquipMessage> {

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

        MainData.MessageBus.Subscribe(this);

        skinnedMeshRenderer = unitModelSpawner.SkinnedMeshRenderer;
        return CameraMeshTargeter.MeshComponentType.SkinnedMeshRenderer;
    }

    public void Handle(ItemEquipMessage message)
    {
        unitModelSpawner.ShouldRespawn();
        skinnedMeshRenderer = unitModelSpawner.SkinnedMeshRenderer;
        InitCameraTargeter();
        Debug.Log("Message handled LOL!");
    }

    public UnitModelSpawner UnitModelSpawner => unitModelSpawner;
}
