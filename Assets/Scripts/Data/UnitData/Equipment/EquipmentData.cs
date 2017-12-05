using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentData {

    [SerializeField, TabGroup("Head"), HideLabel] private EquipmentSlotData head;
    [SerializeField, TabGroup("Torso"), HideLabel] private EquipmentSlotData torso;
    [SerializeField, TabGroup("Legs"), HideLabel] private EquipmentSlotData legs;
    [SerializeField, TabGroup("Left Arm"), HideLabel] private EquipmentSlotData leftArm;
    [SerializeField, TabGroup("Right Arm"), HideLabel] private EquipmentSlotData rightArm;

    public EquipmentSlotData GetEquipmentSlot(EquipmentSlotType type)
    {
        switch (type)
        {
            case EquipmentSlotType.Head:
                return head;
            case EquipmentSlotType.LeftArm:
                return leftArm;
            case EquipmentSlotType.Legs:
                return legs;
            case EquipmentSlotType.RightArm:
                return rightArm;
            case EquipmentSlotType.Torso:
                return torso;
            default:
                return null;
        }
    }

    public bool CanEquip(ItemData item)
    {
        foreach(EquipmentSlotType slotType in item.Slots)
        {
            if (!GetEquipmentSlot(slotType).Empty)
                return false;
        }
        return true;
    }

    public void Equip(ItemData item)
    {
        if (CanEquip(item))
        {
            foreach (EquipmentSlotType slotType in item.Slots)
            {
                GetEquipmentSlot(slotType).Equip(item);
            }
        }
    }

    public ItemData Unequip(EquipmentSlotType slotType)
    {
        EquipmentSlotData slot = GetEquipmentSlot(slotType);
        if (slot.Empty)
        {
            Debug.LogWarning("Slot already empty");
            return null;
        }
        else
        {
            ItemData unequippedItem = slot.UnequipAndReturn();
            foreach (EquipmentSlotType itemSlotType in unequippedItem.Slots)
            {
                GetEquipmentSlot(slotType).Unequip();
            }
            return unequippedItem;
        }
    }

    public List<ItemData> GetEquippedItems()
    {
        var result = new List<ItemData>();
        foreach(EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            ItemData item = GetEquipmentSlot(slotType).Item;
            if (item != null && item.PrimarySlot == slotType)
                result.Add(item);
        }
        return result;
    }
}
