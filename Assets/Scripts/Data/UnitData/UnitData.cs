using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData : IUnitData {

    [SerializeField]
    private string unitName;

    [SerializeField]
    private int level;

    [SerializeField]
    private float height;

    [SerializeField]
    private Attribute[] attributes;

    [SerializeField]
    private bool isInitialized = false;

    public UnitData(UnitDataEditor unitData)
    {
        unitName = unitData.Name;
        level = unitData.Level;

        attributes = new Attribute[(int)Unit.AttributeType.Count];
        attributes[(int)Unit.AttributeType.Size] = new Attribute(unitData.Size);
        attributes[(int)Unit.AttributeType.Hp] = new Attribute(unitData.Hp);
        attributes[(int)Unit.AttributeType.Armor] = new Attribute(unitData.Armor);
        attributes[(int)Unit.AttributeType.Regen] = new Attribute(unitData.Regen);
        attributes[(int)Unit.AttributeType.Damage] = new Attribute(unitData.Damage);
        attributes[(int)Unit.AttributeType.ArmorIgnore] = new Attribute(unitData.ArmorIgnore);
        attributes[(int)Unit.AttributeType.Reach] = new Attribute(unitData.Reach);
        attributes[(int)Unit.AttributeType.MoveSpeed] = new Attribute(unitData.MoveSpeed);
        attributes[(int)Unit.AttributeType.ViewRange] = new Attribute(unitData.ViewRange);

        height = unitData.Height;

        isInitialized = true;
    }

    public string Name
    {
        get
        {
            return unitName;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }
    }

    public Attribute GetAttribute(Unit.AttributeType type)
    {
        return attributes[(int)type];
    }

    public bool IsInitialized
    {
        get { return isInitialized; }
    }
}
