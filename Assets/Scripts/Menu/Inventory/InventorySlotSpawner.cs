using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class InventorySlotSpawner : PrefabSpawner {

    [SerializeField] private InventoryMenu parentMenu;
    [SerializeField] private SpawnedSlotType slotType;
    [SerializeField, ShowIf("IsEq")] private EquipmentSlotType[] eqSlotOrder; 
    [SerializeField, ShowIf("IsEq")] private string[] eqSlotNames; 

    protected override void AfterSpawn()
    {
        if(parentMenu == null)
            parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);

        InventorySlotUI inventorySlotUi = SpawnedInstance.GetComponent<InventorySlotUI>();
        switch (slotType)
        {
            case SpawnedSlotType.Inventory:
                inventorySlotUi.Initialize(parentMenu, new InventoryMenu.ItemSlotID(currentId));
                break;
            case SpawnedSlotType.Equipment:
                inventorySlotUi.Initialize(parentMenu, new InventoryMenu.ItemSlotID(eqSlotOrder[currentId]), eqSlotNames[currentId]);
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
