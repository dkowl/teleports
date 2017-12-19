using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour {

    [SerializeField] UnitData unitData;
    [OdinSerialize, System.NonSerialized] InventoryData inventoryData;
    [SerializeField] InventoryItemSpawner itemSpawner;
    [SerializeField] CameraMeshTargeter cameraTargeter;
    [SerializeField] TextureAtlasFromModels inventoryAtlas;
    [SerializeField] InventorySlotSpawner inventorySlotSpawner;

    private bool isInitialized;
    private Dictionary<ItemData, int> internalItemIds;

    private ItemSlotID selectedSlotId;

    public static event System.Action OnSelectionChangedEvent;

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
        Select(selectedSlotId);
        isInitialized = true;

        inventorySlotSpawner.enabled = false;
        inventorySlotSpawner.ParentMenu = this;
        inventorySlotSpawner.enabled = true;
    }

    public void Select(ItemSlotID itemSlotId)
    {
        //if (itemSlotId != selectedSlotId)
        {
            selectedSlotId = itemSlotId;
            if (SelectedItem != null)
                cameraTargeter.SetTarget(itemSpawner.GetItemMeshFilter(internalItemIds[SelectedItem]));
            OnSelectionChangedEvent();
        }
    }

    public Rect GetItemIconUvRect(ItemData itemData)
    {
        return inventoryAtlas.GetUv(internalItemIds[itemData]);
    }

    public bool IsSelected(ItemSlotID itemSlotId)
    {
        return selectedSlotId == itemSlotId;
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

    public UnitData UnitData
    {
        get { return unitData; }
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

    public ItemData SelectedItem
    {
        get
        {
            if (!selectedSlotId.isEquipmentSlot)
            {
                return inventoryData.GetInventorySlotData(selectedSlotId.inventorySlotId).Item;
            }
            else
            {
                return inventoryData.EquipmentData.GetEquipmentSlot(selectedSlotId.equipmentSlotType).Item;
            }
        }
    }

    [System.Serializable]
    public struct ItemSlotID
    {
        public bool isEquipmentSlot;
        [ShowIf("isEquipmentSlot")] public EquipmentSlotType equipmentSlotType;
        [HideIf("isEquipmentSlot")] public int inventorySlotId;

        public ItemSlotID(int inventorySlotId)
        {
            isEquipmentSlot = false;
            equipmentSlotType = EquipmentSlotType.None;
            this.inventorySlotId = inventorySlotId;
        }

        public ItemSlotID(EquipmentSlotType equipmentSlotType)
        {
            isEquipmentSlot = true;
            this.equipmentSlotType = equipmentSlotType;
            inventorySlotId = 0;
        }

        public static bool operator ==(ItemSlotID x, ItemSlotID y)
        {
            if(x.isEquipmentSlot == y.isEquipmentSlot)
            {
                if (x.isEquipmentSlot)
                    return x.equipmentSlotType == y.equipmentSlotType;
                else
                    return x.inventorySlotId == y.inventorySlotId;
            }
            else
            {
                return false;
            }
                
        }

        public static bool operator !=(ItemSlotID x, ItemSlotID y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            return obj is ItemSlotID && this == (ItemSlotID)obj;
        }

        public override int GetHashCode()
        {
            if (isEquipmentSlot)
                return equipmentSlotType.GetHashCode();
            else
                return inventorySlotId.GetHashCode();
        }
    }
}
