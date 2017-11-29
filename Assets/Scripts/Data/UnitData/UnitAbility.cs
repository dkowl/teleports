using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitAbility {

	public enum Type
    {
        STR,
        DEX,
        INT
    }

    [SerializeField, HideInInspector] Type type;
    [SerializeField, HideLabel] int value;

    public UnitAbility(Type type)
    {
        this.type = type;
        value = 10;
    }

    public string Name
    {
        get { return UnitAbilityData.GetName(type); }
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }
}
