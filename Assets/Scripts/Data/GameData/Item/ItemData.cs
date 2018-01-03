using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ItemData {

    [SerializeField] private string displayName;
    [SerializeField] private string uniqueName;
    [SerializeField, FoldoutGroup("Details", false), EnumToggleButtons] private ItemType typeFlags;
    [SerializeField, FoldoutGroup("Details", false), ShowIf("IsWeapon")] private WeaponData weaponData;
    [SerializeField, FoldoutGroup("Details", false)] private List<Skill> skills;
    [SerializeField, FoldoutGroup("Details", false)] private List<Perk> perks;
    [SerializeField, FoldoutGroup("Details", false)] private EquipmentSlotCombination[] slotCombinations;
    [SerializeField, FoldoutGroup("Details", false)] private ItemGraphics graphics;

    protected ItemData()
    {
        displayName = "New Item";
        uniqueName = "New Item";
    }

    public ItemData(ItemData other)
    {
        displayName = other.displayName;
        uniqueName = other.UniqueName;
        typeFlags = other.typeFlags;
        weaponData = other.weaponData;
        skills = other.skills;
        perks = other.perks;
        slotCombinations = other.slotCombinations;
        graphics = other.graphics;
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
        return uniqueName == other.uniqueName;
        //TODO: compare more accurately than just names            
    }

    public override int GetHashCode()
    {
        return uniqueName.GetHashCode();
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

    public string UniqueName
    {
        get { return uniqueName; }
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
