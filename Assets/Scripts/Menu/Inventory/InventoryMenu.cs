using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour {

    [OdinSerialize, System.NonSerialized] InventoryData inventoryData;
    [SerializeField] InventoryItemSpawner itemSpawner;
    [SerializeField] CameraMeshTargeter cameraTargeter;
    [SerializeField] TextureAtlasFromModels inventoryAtlas;
    [SerializeField] InventorySlotSpawner inventorySlotSpawner;

    private bool isInitialized;
    private Dictionary<ItemData, int> internalItemIds;


    private void OnEnable()
    {
        internalItemIds = new Dictionary<ItemData, int>();

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

    private void Start()
    {
        inventoryAtlas = new TextureAtlasFromModels(Utils.GetComponentsInObjects<MeshFilter>(itemSpawner.SpawnedItems).ToArray(), cameraTargeter);

        isInitialized = true;

        inventorySlotSpawner.enabled = false;
        inventorySlotSpawner.ParentMenu = this;
        inventorySlotSpawner.enabled = true;
    }

    public Rect GetItemIconUvRect(ItemData itemData)
    {
        return inventoryAtlas.GetUv(internalItemIds[itemData]);
    }

    private void InitItemSpawner()
    {
        var itemPrefabs = new List<GameObject>();
        int internalItemId = 0;
        foreach(var item in inventoryData.GetAllItemsInInventory())
        {
            itemPrefabs.Add(item.Graphics.Prefab);
            internalItemIds[item] = internalItemId;
            internalItemId++;
        }
        itemSpawner = new InventoryItemSpawner(itemPrefabs);
    }

    public InventoryData InventoryData
    {
        get { return inventoryData; }
    }

    public bool IsInitialized
    {
        get { return isInitialized; }
    }

    public Texture2D ItemIconAtlas
    {
        get { return inventoryAtlas.Atlas; }
    }

}
