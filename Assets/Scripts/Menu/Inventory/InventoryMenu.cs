using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InventoryMenu : SerializedMonoBehaviour {

    InventoryData inventoryData;
    InventoryItemSpawner itemSpawner;

    void OnEnable()
    {
        inventoryData = MainData.CurrentPlayerData.InventoryData;
        InitItemSpawner();
        itemSpawner.Spawn();
    }

    void InitItemSpawner()
    {
        if(itemSpawner == null)
        {
            var itemPrefabs = new List<GameObject>();
            foreach(var item in inventoryData.GetAllItemsInInventory())
            {
                itemPrefabs.Add(item.Graphics.Prefab);
            }
            itemSpawner = new InventoryItemSpawner(itemPrefabs);
        }
    }

}
