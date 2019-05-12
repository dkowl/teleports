using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Teleports.Utils;

public class InventoryMenu : SerializedMonoBehaviour, 
    IMessageHandler<ItemEquipMessage>
{

    [SerializeField] private UnitData unitData;
    [SerializeField] private InventoryItemSpawner itemSpawner;
    [SerializeField] private CameraMeshTargeter cameraTargeter;
    [SerializeField] private TextureAtlasFromModels inventoryAtlas;

    private bool isInitialized;
    private Dictionary<ItemData, int> internalItemIds;

    private ItemSlotID selectedSlotId;

    private EquipmentSlotType lastEquippedSlotType;
    
    public static event System.Action UpdateUiEvent;

    private void OnEnable()
    {
        unitData = Main.GameState.CurrentHeroData.UnitData;

        internalItemIds = new Dictionary<ItemData, int>();

        Main.MessageBus.Subscribe(this);

        InitItemSpawner();       
    }

    private void OnDestroy()
    {
        itemSpawner.Despawn();
        Main.MessageBus.Unsubscribe(this);
    }

    private void Start()
    {
        BuildTextureAtlas();
        Select(selectedSlotId);
        isInitialized = true;
    }

    private void BuildTextureAtlas()
    {
        inventoryAtlas = new TextureAtlasFromModels(Utils.GetComponentsInObjects<MeshFilter>(itemSpawner.SpawnedItems).ToArray(), cameraTargeter);
    }

    public void Select(ItemSlotID itemSlotId)
    {
        selectedSlotId = itemSlotId;
        if (SelectedItem != null)
            cameraTargeter.SetTarget(itemSpawner.GetItemMeshFilter(internalItemIds[SelectedItem]));
        UpdateUiEvent?.Invoke();        
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
        Select(new ItemSlotID(lastEquippedSlotType));
    }

    public void UnequipSelected()
    {
        ItemData item = InventoryData.EquipmentData.GetEquipmentSlot(selectedSlotId.equipmentSlotType).Item;
        InventoryData.Unequip(selectedSlotId.equipmentSlotType);
        Select(new ItemSlotID(InventoryData.InventorySlotIdOf(item)));
    }

    public void Handle(ItemEquipMessage message)
    {
        if (message.Type == ItemEquipMessage.EventType.Equip)
        {
            lastEquippedSlotType = message.EqSlotType;
        }
    }

    private void InitItemSpawner()
    {
        var itemPrefabs = new List<GameObject>();
        int internalItemId = 0;
        foreach(var item in InventoryData.GetAllItems())
        {
            itemPrefabs.Add(Main.StaticData.Graphics.ItemGraphics.GetValue(item.GraphicsId).Prefab);
            internalItemIds[item] = internalItemId;
            internalItemId++;
        }
        itemSpawner.Despawn();
        itemSpawner = new InventoryItemSpawner(itemPrefabs);
        itemSpawner.Spawn();
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
