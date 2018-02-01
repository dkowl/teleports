using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentData {

    [SerializeField, TabGroup("Head"), HideLabel] private EquipmentSlotData head;
    [SerializeField, TabGroup("Torso"), HideLabel] private EquipmentSlotData torso;
    [SerializeField, TabGroup("Legs"), HideLabel] private EquipmentSlotData legs;
    [SerializeField, TabGroup("Feet"), HideLabel] private EquipmentSlotData feet;
    [SerializeField, TabGroup("Left Arm"), HideLabel] private EquipmentSlotData leftArm;
    [SerializeField, TabGroup("Right Arm"), HideLabel] private EquipmentSlotData rightArm;

    public EquipmentData()
    {
        head = new EquipmentSlotData();
        torso = new EquipmentSlotData();
        legs = new EquipmentSlotData();
        feet = new EquipmentSlotData();
        leftArm = new EquipmentSlotData();
        rightArm = new EquipmentSlotData();
    }

    public void CorrectInvalidData()
    {
        foreach(EquipmentSlotData eqSlotData in GetEquipmentSlots())
        {
            eqSlotData.CorrectInvalidData();
        }
    }

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
            case EquipmentSlotType.Feet:
                return feet;
            case EquipmentSlotType.RightArm:
                return rightArm;
            case EquipmentSlotType.Torso:
                return torso;
            default:
                return null;
        }
    }

    public CanEquipResult CanEquip(EquipmentSlotCombination slotCombination)
    {
        var result = new CanEquipResult();
        bool primaryConflictDetected = false;
        bool secondaryConflictDetected = false;
        foreach(var slotType in slotCombination.SlotsTaken)
        {
            EquipmentSlotData slotData = GetEquipmentSlot(slotType);
            if(slotData == null)
            {
                result.Status = CanEquipStatus.No_Impossible;
                return result;
            }
            if (!slotData.Empty)
            {
                result.AddConflictingItem(slotData.Item);
                if (slotType == slotCombination.PrimarySlot)
                    primaryConflictDetected = true;
                else
                    secondaryConflictDetected = true;
            }
        }
        if (primaryConflictDetected && !secondaryConflictDetected)
            result.Status = CanEquipStatus.No_PrimaryConflict;
        else if (secondaryConflictDetected)
            result.Status = CanEquipStatus.No_SecondaryConflict;
        else
            result.Status = CanEquipStatus.Yes;

        return result;
    }

    public CanEquipResult CanEquip(ItemData item)
    {
        CanEquipResult bestResult = new CanEquipResult();
        foreach(var slotCombination in item.SlotCombinations)
        {
            CanEquipResult currentResult = CanEquip(slotCombination);
            if (currentResult.IsBetterThan(bestResult))
                bestResult = currentResult;
        }
        return bestResult;
    }

    public void Equip(ItemData item)
    {
        foreach (var slotCombination in item.SlotCombinations)
        {
            if (CanEquip(slotCombination).Status == CanEquipStatus.Yes)
            {
                foreach (EquipmentSlotType slotType in slotCombination.SlotsTaken)
                {
                    bool asPrimary = (slotType == slotCombination.PrimarySlot);
                    GetEquipmentSlot(slotType).Equip(item, asPrimary);
                }
                MainData.MessageBus.Publish(new ItemEquipMessage(ItemEquipMessage.EventType.Equip, item, slotCombination.PrimarySlot));
                return;
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
            EquipmentSlotCombination slotCombination;
            ItemData item = slot.Item;
            if (slot.Primary)
            {
                slotCombination = item.GetSlotCombination(slotType);
            }
            else
            {
                slotCombination = item.GetSlotCombinationWithSecondarySlot(slotType);
            }
            return Unequip(slotCombination);
        }
    }

    public void Unequip(ItemData item)
    {
        foreach(var slotType in GetValidSlotTypes())
        {
            EquipmentSlotData slot = GetEquipmentSlot(slotType);
            if(slot.Item == item && slot.Primary)
            {
                Unequip(item.GetSlotCombination(slotType));
                return;
            }
        }
    }

    private ItemData Unequip(EquipmentSlotCombination slotCombination)
    {
        foreach(var slotType in slotCombination.SlotsTaken)
        {
            if(slotType != slotCombination.PrimarySlot)
            {
                GetEquipmentSlot(slotType).Unequip();
            }
        }
        ItemData result = GetEquipmentSlot(slotCombination.PrimarySlot).UnequipAndReturn();
        MainData.MessageBus.Publish(new ItemEquipMessage(ItemEquipMessage.EventType.Unequip, result, slotCombination.PrimarySlot));
        return result;
    }

    public IList<EquippedItemInfo> GetEquippedItems()
    {
        var result = new List<EquippedItemInfo>();
        foreach(EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            if (slotType == EquipmentSlotType.None) continue;
            EquipmentSlotData slotData = GetEquipmentSlot(slotType);
            ItemData item = slotData.Item;
            if (item != null && slotData.Primary)
                result.Add(new EquippedItemInfo(item, slotType));
        }
        return result.AsReadOnly();
    }

    public IList<EquipmentSlotData> GetEquipmentSlots()
    {
        var result = new List<EquipmentSlotData>();
        foreach (EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            if(slotType != EquipmentSlotType.None)
            {
                result.Add(GetEquipmentSlot(slotType));
            }
        }
        return result;
    }

    public IList<EquipmentSlotType> GetValidSlotTypes()
    {
        var result = new List<EquipmentSlotType>();
        foreach(EquipmentSlotType slotType in System.Enum.GetValues(typeof(EquipmentSlotType)))
        {
            if (slotType != EquipmentSlotType.None)
            {
                result.Add(slotType);
            }
        }
        return result;
    }

    public enum CanEquipStatus
    {
        Yes,
        No_PrimaryConflict,
        No_SecondaryConflict,
        No_Impossible
    }

    public class CanEquipResult
    {
        private CanEquipStatus status;
        private List<ItemData> conflictingItems;

        public CanEquipResult()
        {
            status = CanEquipStatus.No_Impossible;
            conflictingItems = new List<ItemData>();
        }

        public void AddConflictingItem(ItemData item)
        {
            conflictingItems.Add(item);
        }

        public bool IsBetterThan(CanEquipResult other)
        {
            if (status != other.status)
                return status < other.status;
            else
                return conflictingItems.Count < other.ConflictingItems.Count;
        }

        public CanEquipStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public IList<ItemData> ConflictingItems
        {
            get { return conflictingItems.AsReadOnly(); }
        }
    }

    public class EquippedItemInfo
    {
        private ItemData item;
        private EquipmentSlotType primarySlot;

        public EquippedItemInfo(ItemData item, EquipmentSlotType primarySlot)
        {
            this.item = item;
            this.primarySlot = primarySlot;
        }

        public ItemData Item
        {
            get { return item; }
        }

        public EquipmentSlotType PrimarySlot
        {
            get { return primarySlot; }
        }
    }
}
