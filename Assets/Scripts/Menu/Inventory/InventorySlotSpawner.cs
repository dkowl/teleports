using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotSpawner : PrefabSpawner {

    [SerializeField] private InventoryMenu parentMenu;
    [SerializeField] private SpawnedSlotType slotType;
    [SerializeField] private EquipmentSlotType[] eqSlotOrder; 

    protected override void AfterSpawn()
    {
        if(parentMenu == null)
            parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);

        InventorySlotUI inventorySlotUI = SpawnedInstance.GetComponent<InventorySlotUI>();
        switch (slotType)
        {
            case SpawnedSlotType.Inventory:
                inventorySlotUI.Initialize(parentMenu, new InventoryMenu.ItemSlotID(currentId));
                break;
            case SpawnedSlotType.Equipment:
                inventorySlotUI.Initialize(parentMenu, new InventoryMenu.ItemSlotID(eqSlotOrder[currentId]));
                break;
        }
    }

    public InventoryMenu ParentMenu
    {
        set { parentMenu = value; }
    }

    public enum SpawnedSlotType
    {
        Inventory,
        Equipment
    }
}
