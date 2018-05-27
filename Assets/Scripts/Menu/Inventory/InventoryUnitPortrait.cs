using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class InventoryUnitPortrait : PortraitUI, IMessageHandler<ItemEquipMessage> {

    [SerializeField] UnitModelSpawner unitModelSpawner;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MainData.MessageBus.Unsubscribe(this);
    }

    protected override CameraMeshTargeter.MeshComponentType SpawnModel()
    {
        InventoryMenu parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);
        gameObject.InitComponent(ref unitModelSpawner);
        unitModelSpawner.AddSpawnData();
        unitModelSpawner.SetPositionOffset(SpecialSpawnPlaces.InventoryPlayer);
        unitModelSpawner.SetRotationOffset(new Vector3(0, 180, 0));
        unitModelSpawner.UnitData = parentMenu.UnitData;
        unitModelSpawner.SpawnAll();

        MainData.MessageBus.Subscribe(this);

        skinnedMeshRenderer = unitModelSpawner.SkinnedMeshRenderer;
        return CameraMeshTargeter.MeshComponentType.SkinnedMeshRenderer;
    }

    protected override void InitCameraTargeter()
    {
        base.InitCameraTargeter();
        CamMeshTargeter.SetPaddingPercentage(0.5f);
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
