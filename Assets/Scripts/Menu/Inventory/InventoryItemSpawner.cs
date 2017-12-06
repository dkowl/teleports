using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSpawner  {

    List<GameObject> itemPrefabs;
    List<GameObject> spawnedItems;

    public InventoryItemSpawner(List<GameObject> itemPrefabs)
    {
        this.itemPrefabs = itemPrefabs;
        spawnedItems = new List<GameObject>();
    }

    public void Spawn()
    {
        WorldPositionGrid spawnPlace = SpecialSpawnPlaces.ItemSpawnPlace;
        Despawn();

        foreach(GameObject itemPrefab in itemPrefabs)
        {
            GameObject spawnedItem = GameObject.Instantiate(itemPrefab) as GameObject;
            spawnedItem.transform.position = spawnPlace.CurrentPosition;
            spawnPlace.GoToNextPosition();
        }
    }

    public void Despawn()
    {
        foreach(var item in spawnedItems)
        {
            if (item != null)
                GameObject.Destroy(item);
        }
    }
}
