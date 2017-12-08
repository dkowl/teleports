using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour {

    [OdinSerialize, System.NonSerialized] public InventoryData inventoryData;
    [SerializeField] InventoryItemSpawner itemSpawner;
    [SerializeField] CameraMeshTargeter cameraTargeter;
    [SerializeField] TextureAtlasFromModels inventoryAtlas;
    [SerializeField] InventorySlotSpawner inventorySlotSpawner;


    void OnEnable()
    {
        inventoryData = new InventoryData();
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        inventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));

        InitItemSpawner();
        itemSpawner.Spawn();        
    }

    void Start()
    {
        inventoryAtlas = new TextureAtlasFromModels(Utils.GetComponentsInObjects<MeshFilter>(itemSpawner.SpawnedItems).ToArray(), cameraTargeter);

        inventorySlotSpawner.enabled = false;
        inventorySlotSpawner.Atlas = inventoryAtlas.Atlas;
        inventorySlotSpawner.Uvs = inventoryAtlas.Uvs;
        inventorySlotSpawner.enabled = true;
    }

    void InitItemSpawner()
    {
        var itemPrefabs = new List<GameObject>();
        foreach(var item in inventoryData.GetAllItemsInInventory())
        {
            itemPrefabs.Add(item.Graphics.Prefab);
        }
        itemSpawner = new InventoryItemSpawner(itemPrefabs);
    }

}
