using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPortraitRotatorButton : RotatorButton{

    [SerializeField] InventoryUnitPortrait unitPortrait;

    private void Start()
    {
        UpdateRotationTarget();
        unitPortrait.UnitModelSpawner.onSpawnEvent += UpdateRotationTarget;
    }

    private void UpdateRotationTarget()
    {
        Debug.Assert(unitPortrait != null);
        rotationTarget = unitPortrait.UnitModelSpawner.UnitTransform;
    }
}
