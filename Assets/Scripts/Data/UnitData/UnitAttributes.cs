using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnitAttribute = Attribute;

[System.Serializable]
public class UnitAttributes{

    public enum Type
    {
        Size,
        Height,
        Hp,
        Armor,
        Regen,
        MoveSpeed,
        RotationSpeed,
        Reach,
        ViewRange
    }

    [SerializeField, InlineProperty] UnitAttribute size;
    [SerializeField, InlineProperty] UnitAttribute height;
    [SerializeField, InlineProperty] UnitAttribute healthPoints;
    [SerializeField, InlineProperty] UnitAttribute armor;
    [SerializeField, InlineProperty] UnitAttribute regeneration;
    [SerializeField, InlineProperty] UnitAttribute movementSpeed;
    [SerializeField, InlineProperty] UnitAttribute rotationSpeed;
    [SerializeField, InlineProperty] UnitAttribute reach;
    [SerializeField, InlineProperty] UnitAttribute viewRange;

    public UnitAttributes()
    {
        size = new UnitAttribute(0.5f);
        height = new UnitAttribute(1.8f);
        healthPoints = new UnitAttribute(100);
        movementSpeed = new UnitAttribute(10);
        rotationSpeed = new UnitAttribute(2);
        viewRange = new UnitAttribute(30);
    }

    public UnitAttribute GetAttribute(Type type)
    {
        switch (type)
        {
            case Type.Armor:
                return armor;
            case Type.Height:
                return height;
            case Type.Hp:
                return healthPoints;
            case Type.MoveSpeed:
                return movementSpeed;
            case Type.Reach:
                return reach;
            case Type.Regen:
                return regeneration;
            case Type.RotationSpeed:
                return rotationSpeed;
            case Type.Size:
                return size;
            case Type.ViewRange:
                return viewRange;
            default:
                return new UnitAttribute();
        }
    }

    public UnitAttribute Size
    {
        get { return size; }
    }

    public UnitAttribute Height
    {
        get { return height; }
    }

    public UnitAttribute HealthPoints
    {
        get { return healthPoints; }
    }

    public UnitAttribute Armor
    {
        get { return armor; }
    }

    public UnitAttribute Regeneration
    {
        get { return regeneration; }
    }

    public UnitAttribute MovementSpeed
    {
        get { return movementSpeed; }
    }

    public UnitAttribute RotationSpeed
    {
        get { return rotationSpeed; }
    }

    public UnitAttribute Reach
    {
        get { return reach; }
    }

    public UnitAttribute ViewRange
    {
        get { return viewRange; }
    }
}
