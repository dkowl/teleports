using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour, IMessageHandler<ItemEquipMessage> {

    [SerializeField] UnitData unitData;
    [SerializeField] InventoryItemSpawner itemSpawner;
    [SerializeField] CameraMeshTargeter cameraTargeter;
    [SerializeField] TextureAtlasFromModels inventoryAtlas;
    [SerializeField] InventorySlotSpawner inventorySlotSpawner;

    private bool isInitialized;
    private Dictionary<ItemData, int> internalItemIds;

    private ItemSlotID selectedSlotId;
    
    public static event System.Action UpdateUiEvent;

    private void OnEnable()
    {
        internalItemIds = new Dictionary<ItemData, int>();
        
        InventoryData.Add(MainData.CurrentGameData.GetItem("Greatsword"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("Handaxe"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("Warhammer"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("DoubleAxe"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("Greataxe"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("Longsword"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("Mace"));
        InventoryData.Add(MainData.CurrentGameData.GetItem("ShortSword"));

        MainData.MessageBus.Subscribe(this);

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
            if (UpdateUiEvent != null)
            {
                UpdateUiEvent();
            }
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

    public void EquipSelected()
    {
        InventoryData.Equip(SelectedItem);
    }

    public void UnequipSelected()
    {
        InventoryData.Unequip(selectedSlotId.equipmentSlotType);
    }

    public void Handle(ItemEquipMessage message)
    {
        if (message.Type == ItemEquipMessage.EventType.Equip)
        {
            Select(new ItemSlotID(message.EqSlotType));
        }
        if (UpdateUiEvent != null)
        {
            UpdateUiEvent();
        }
    }

    private void InitItemSpawner()
    {
        var itemPrefabs = new List<GameObject>();
        int internalItemId = 0;
        foreach(var item in InventoryData.GetAllItems())
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
        get { return UnitData.Inventory; }
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
                return InventoryData.GetInventorySlotData(selectedSlotId.inventorySlotId).Item;
            }
            else
            {
                return InventoryData.EquipmentData.GetEquipmentSlot(selectedSlotId.equipmentSlotType).Item;
            }
        }
    }

    public ItemSlotID SelectedSlotId
    {
        get { return selectedSlotId; }
    }

    public bool IsInventorySlotSelected
    {
        get
        {
            return !selectedSlotId.isEquipmentSlot;
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
