using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class UnitData {

    private const int labelWidth = 110;

    [SerializeField, PropertyOrder(-5), LabelWidth(labelWidth)] private string unitName;
    [SerializeField, PropertyOrder(-4), LabelWidth(labelWidth)] private int level;
    [SerializeField, InlineProperty, LabelWidth(labelWidth)] private UnitAbilities abilities;    
    [SerializeField, InlineProperty, LabelWidth(labelWidth)] private UnitAttributes attributes;
    [SerializeField, InlineProperty, LabelWidth(labelWidth)] private SkillID mainAttack;
    [SerializeField, LabelWidth(labelWidth)] private List<string> perks;
    [SerializeField, LabelWidth(labelWidth)] private List<SkillID> skills;
    [SerializeField, HideInInspector] private bool isInitialized;

    public UnitData()
    {
        isInitialized = false;
        Initialize();
    }

    [Button, HideIf("IsInitialized")]
    public void Initialize()
    {
        unitName = "New Unit";
        level = 1;
        perks = new List<string>();
        skills = new List<SkillID>();
        isInitialized = true;
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

    public float Height
    {
        get
        {
            return GetAttribute(UnitAttributes.Type.Height).Value;
        }
    }

    public Attribute GetAttribute(UnitAttributes.Type type)
    {
        return attributes.GetAttribute(type);
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
