using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public partial class ItemData {
    
    protected ItemData()
    {
        displayName = DataDefaults.itemName;
        uniqueName = DataDefaults.itemName;
        typeFlags = ItemType.None;
        weaponData = new WeaponData();
        skills = new List<SkillID>();
        perks = new List<PerkID>();
    }

    public void CorrectInvalidData()
    {
        if(graphicsId == null)
        {
            Debug.LogWarning("Item graphics not found, loading defaults...");
            graphics = MainData.Defaults.itemGraphics;

            graphicsId = new ItemGraphicsID(graphics.UniqueName);
        }

        WeaponData.CorrectInvalidData();
    }

    public ItemData(ItemData other, string uniqueName) :
        this(other)
    {
        this.uniqueName = uniqueName;
    }

    public static bool operator == (ItemData data1, ItemData data2)
    {
        if ((object)data1 == null)
            return (object)data2 == null;

        else return data1.Equals(data2);
    }

    public static bool operator != (ItemData data1, ItemData data2)
    {
        return !(data1 == data2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        ItemData other = (ItemData)obj;
        //Debug.Log("Comparing (displayName) " + displayName + " vs " + other.displayName);
        //Debug.Log("Comparing (uniqueName) " + uniqueName + " vs " + other.uniqueName);
        return 
            uniqueName == other.uniqueName &&
            graphicsId == other.graphicsId;
        //TODO: compare more accurately than just names            
    }

    public override int GetHashCode()
    {
        return uniqueName.GetHashCode() ^ (graphicsId != null ? graphicsId.GetHashCode() : 0);
        //TODO: calculate more accurately than just names     
    }

    public bool IsType(ItemType type)
    {
        return (type & typeFlags) == type;
    }

    public EquipmentSlotCombination GetSlotCombination(EquipmentSlotType primarySlot)
    {
        foreach(var slotCombination in slotCombinations)
        {
            if(slotCombination.PrimarySlot == primarySlot)
            {
                return slotCombination;
            }
        }
        return null;
    }

    public EquipmentSlotCombination GetSlotCombinationWithSecondarySlot(EquipmentSlotType secondarySlot)
    {
        foreach (var slotCombination in slotCombinations)
        {
            if (slotCombination.SecondarySlots.Contains(secondarySlot))
            {
                return slotCombination;
            }
        }
        return null;
    }

    //properties for [ShowIf] Odin Attribute
    bool IsWeapon => IsType(ItemType.Weapon);
}
