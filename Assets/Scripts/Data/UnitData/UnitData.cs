using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class UnitData : IUnitData {

    [SerializeField, PropertyOrder(-5)]
    private string unitName;

    [SerializeField, PropertyOrder(-4)]
    private int level;

    [SerializeField, HideInInspector]
    private UnitAbility[] abilities;

    [ListDrawerSettings(ListElementLabelName = "Name", IsReadOnly = true)]
    [SerializeField]
    [InlineProperty]
    private UnitAttribute[] attributes;

    [SerializeField]
    private SkillID mainAttack;

    [SerializeField]
    private bool isInitialized = false;

    public UnitData()
    {
        Initialize();
    }

    [Button]
    public void Initialize()
    {
        var abilityTypes = (UnitAbility.Type[])System.Enum.GetValues(typeof(UnitAbility.Type));
        abilities = new UnitAbility[abilityTypes.Length];
        for(int i = 0; i<abilities.Length; i++)
        {
            abilities[i] = new UnitAbility(abilityTypes[i]);
        }

        var attributeTypes = (UnitAttribute.Type[])System.Enum.GetValues(typeof(UnitAttribute.Type));
        attributes = new UnitAttribute[attributeTypes.Length];
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i] = new UnitAttribute(attributeTypes[i]);
        }
    }

    public string Name
    {
        get { return unitName; }
        set { unitName = value; }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    [ShowInInspector, GUIColor(1, 0.5f, 0.5f), PropertyOrder(-3)]
    public int Strength
    {
        get { return abilities[(int)UnitAbility.Type.STR].Value; }
        set { abilities[(int)UnitAbility.Type.STR].Value = value; }
    }

    [ShowInInspector, GUIColor(0.5f, 1, 0.5f), PropertyOrder(-2)]
    public int Dexterity
    {
        get { return abilities[(int)UnitAbility.Type.DEX].Value; }
        set { abilities[(int)UnitAbility.Type.STR].Value = value; }
    }

    [ShowInInspector, GUIColor(0.5f, 0.5f, 1), PropertyOrder(-1)]
    public int Intelligence
    {
        get { return abilities[(int)UnitAbility.Type.INT].Value; }
        set { abilities[(int)UnitAbility.Type.STR].Value = value; }
    }

    public float Height
    {
        get
        {
            return GetAttribute(UnitAttribute.Type.Height).Value;
        }
    }

    public Attribute GetAttribute(UnitAttribute.Type type)
    {
        return attributes[(int)type];
    }

    public bool IsInitialized
    {
        get { return isInitialized; }
    }

    public SkillID MainAttack
    {
        get { return mainAttack; }
    }
}
