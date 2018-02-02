using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitSpawner  {

	public static void SpawnItems(this Unit unit)
    {
        GameObject items = new GameObject("Items");
        items.transform.parent = unit.transform;

        foreach (var itemInfo in unit.UnitData.Inventory.GetEquippedItems())
        {
            Debug.Log("Spawning " + itemInfo.Item.DisplayName);
            ItemData itemData = itemInfo.Item;
            GameObject itemObject = new GameObject(itemData.DisplayName);
            itemObject.transform.parent = items.transform;
            Item item = itemObject.AddComponent<Item>();
            item.Data = itemData;
            item.PrimarySlot = itemInfo.PrimarySlot;
        }
    }
}
