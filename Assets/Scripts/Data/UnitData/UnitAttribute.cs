using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitAttribute : Attribute {

    public enum Type
    {
        Size,
        Hp,
        Armor,
        Regen,
        Damage,
        ArmorIgnore,
        Reach,
        MoveSpeed,
        ViewRange,
        RotationSpeed,
        Height
    }

    [SerializeField, HideInInspector] Type type;

    public UnitAttribute(Type type)
    {
        this.type = type;
    }

    public string Name
    {
        get { return UnitAttributeData.GetName(type); }
    }

}
