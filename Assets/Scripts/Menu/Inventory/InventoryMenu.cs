using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour, IMessageHandler<ItemEquipMessage> {

    [SerializeField] private UnitData unitData;
    [SerializeField] private InventoryItemSpawner itemSpawner;
    [SerializeField] private CameraMeshTargeter cameraTargeter;
    [SerializeField] private TextureAtlasFromModels inventoryAtlas;
    [SerializeField] private InventorySlotSpawner inventorySlotSpawner;

    private bool isInitialized;
    private Dictionary<ItemData, int> internalItemIds;

    private ItemSlotID selectedSlotId;
    
    public static event System.Action UpdateUiEvent;

    private void OnEnable()
    {
        unitData = MainData.CurrentPlayerData.UnitData;

        internalItemIds = new Dictionary<ItemData, int>();

        //InventoryData.Add(MainData.Game.GetItem("Dagger"));
        /*InventoryData.Add(MainData.Game.GetItem("Greatsword"));
        InventoryData.Add(MainData.Game.GetItem("Handaxe"));
        InventoryData.Add(MainData.Game.GetItem("Warhammer"));
        InventoryData.Add(MainData.Game.GetItem("DoubleAxe"));
        InventoryData.Add(MainData.Game.GetItem("Greataxe"));
        InventoryData.Add(MainData.Game.GetItem("Longsword"));
        InventoryData.Add(MainData.Game.GetItem("Mace"));
        InventoryData.Add(MainData.Game.GetItem("ShortSword"));*/

        MainData.MessageBus.Subscribe(this);

        InitItemSpawner();
        itemSpawner.Spawn();        
    }

    private void OnDestroy()
    {
        itemSpawner.Despawn();
        MainData.MessageBus.Unsubscribe(this);
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
        else if(message.Type == ItemEquipMessage.EventType.Unequip)
        {
            Select(new ItemSlotID(InventoryData.InventorySlotIdOf(message.Item)));
        }

        UpdateUiEvent?.Invoke();
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

    public UnitData UnitData => unitData;
    public InventoryData InventoryData => UnitData.Inventory;
    public bool IsInitialized => isInitialized;
    public Texture2D ItemIconAtlas => inventoryAtlas.Atlas;
    public ItemSlotID SelectedSlotId => selectedSlotId;
    public bool IsInventorySlotSelected => !selectedSlotId.isEquipmentSlot;
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
