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
    [SerializeField, FoldoutGroup("Details", false)] private EquipmentSlotCombination[] slotCombinations;
    [SerializeField, FoldoutGroup("Details", false)] private ItemGraphics graphics;

    protected ItemData()
    {
        displayName = "New Item";
    }

    public ItemData(string displayName, List<Skill> skills, List<Perk> perks, EquipmentSlotCombination[] slotCombinations, ItemGraphics graphics)
    {
        this.displayName = displayName;
        this.skills = skills;
        this.perks = perks;
        this.slotCombinations = slotCombinations;
        this.graphics = graphics;
    }

    public ItemData(ItemData other)
    {
        displayName = other.displayName;
        typeFlags = other.typeFlags;
        weaponData = other.weaponData;
        skills = other.skills;
        perks = other.perks;
        slotCombinations = other.slotCombinations;
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

    public WeaponData WeaponData
    {
        get { return weaponData; }
    }

    public List<Perk> Perks
    {
        get { return perks; }
    }

    public IList<EquipmentSlotCombination> SlotCombinations
    {
        get { return System.Array.AsReadOnly(slotCombinations); }
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
