using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotSpawner : PrefabSpawner {

    [SerializeField] private InventoryMenu parentMenu;

    protected override void AfterSpawn()
    {
        if(parentMenu == null)
            parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);

        InventorySlotUI inventorySlotUI = SpawnedInstance.GetComponent<InventorySlotUI>();
        inventorySlotUI.Initialize(parentMenu, currentId);
    }

    public InventoryMenu ParentMenu
    {
        set { parentMenu = value; }
    }
}
