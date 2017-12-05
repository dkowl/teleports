using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ItemData {

    [SerializeField] private string displayName;

    [SerializeField, FoldoutGroup("Details", false), EnumToggleButtons] private ItemType typeFlags;
    [SerializeField, FoldoutGroup("Details", false), ShowIf("IsWeapon")] private WeaponData weaponData;
    [SerializeField, FoldoutGroup("Details", false)] private List<Skill> skills;
    [SerializeField, FoldoutGroup("Details", false)] private List<Perk> perks;
    [SerializeField, FoldoutGroup("Details", false)] private EquipmentSlotType[] slots;
    [SerializeField, FoldoutGroup("Details", false)] private EquipmentSlotType primarySlot;
    [SerializeField, FoldoutGroup("Details", false)] private ItemGraphics graphics;

    protected ItemData()
    {
        displayName = "New Item";
    }

    public ItemData(string displayName, List<Skill> skills, List<Perk> perks, EquipmentSlotType[] slots, ItemGraphics graphics)
    {
        this.displayName = displayName;
        this.skills = skills;
        this.perks = perks;
        this.slots = slots;
        this.graphics = graphics;
    }

    public ItemData(ItemData other)
    {
        displayName = other.displayName;
        skills = other.skills;
        perks = other.perks;
        slots = other.slots;
        primarySlot = other.primarySlot;
        graphics = other.graphics;
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
        return
            displayName == other.displayName;
        //TODO: compare more accurately than just names            
    }

    public override int GetHashCode()
    {
        return displayName.GetHashCode();
        //TODO: calculate more accurately than just names     
    }

    public bool IsType(ItemType type)
    {
        return (type & typeFlags) == type;
    }

    public string DisplayName
    {
        get { return displayName; }
    }

    public List<Perk> Perks
    {
        get { return perks; }
    }

    public EquipmentSlotType[] Slots
    {
        get { return slots; }
    }

    public EquipmentSlotType PrimarySlot
    {
        get { return primarySlot; }
    }

    public ItemGraphics Graphics
    {
        get { return graphics; }
    }

    //properties for [ShowIf] Odin Attribute
    bool IsWeapon
    {
        get { return IsType(ItemType.Weapon); }
    }

}
