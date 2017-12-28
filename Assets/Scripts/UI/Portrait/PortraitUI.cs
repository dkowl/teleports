using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

[ExecuteInEditMode]
public class PortraitUI : MonoBehaviour {

    private RawImage rawImage;
    [SerializeField] private ObjectType objectType;
    private UnitModelSpawner unitModelSpawner;

    private void OnEnable()
    {
        InitRawTexture();
        SpawnModel();
    }

    private void InitRawTexture()
    {
        gameObject.InitComponent(ref rawImage);
    }

    private void SpawnModel()
    {
        switch (objectType)
        {
            case ObjectType.InventoryCurrentUnit:
                gameObject.InitComponent(ref unitModelSpawner);
                InventoryMenu parentMenu = GetComponentInParent<InventoryMenu>();
                Debug.Assert(parentMenu != null);
                unitModelSpawner.SetPositionOffset(SpecialSpawnPlaces.InventoryPlayer);
                unitModelSpawner.UnitData = parentMenu.UnitData;
                break;
        }
    }

    public enum ObjectType
    {
        InventoryCurrentUnit
    }
}
