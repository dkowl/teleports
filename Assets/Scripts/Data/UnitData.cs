using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "unitData", menuName = "Custom/UnitData", order = 1)]
public class UnitData : ScriptableObject, IUnitData
{
    [SerializeField]
    private string unitName;

    [FormerlySerializedAs("level_")]
    [SerializeField]
    private int level;

    [FormerlySerializedAs("size_")]
    [SerializeField]
    private float size;

    [FormerlySerializedAs("hp_")]
    [SerializeField]
    private float hp;

    [FormerlySerializedAs("armor_")]
    [SerializeField]
    private float armor;

    [FormerlySerializedAs("regen_")]
    [SerializeField]
    private float regen;

    [FormerlySerializedAs("damage_")]
    [SerializeField]
    private float damage;

    [FormerlySerializedAs("armorIgnore_")]
    [SerializeField]
    private float armorIgnore;

    [FormerlySerializedAs("reach_")]
    [SerializeField]
    private float reach;

    [FormerlySerializedAs("moveSpeed_")]
    [SerializeField]
    private float moveSpeed;

    [FormerlySerializedAs("viewRange_")]
    [SerializeField]
    private float viewRange;

    [FormerlySerializedAs("height_")]
    [SerializeField]
    private float height;

    #region interface implementation
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

    public float Size
    {
        get
        {
            return size;
        }
    }

    public float Hp
    {
        get
        {
            return hp;
        }
    }

    public float Armor
    {
        get
        {
            return armor;
        }
    }

    public float Regen
    {
        get
        {
            return regen;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }
    }

    public float ArmorIgnore
    {
        get
        {
            return armorIgnore;
        }
    }

    public float Reach
    {
        get
        {
            return reach;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public float ViewRange
    {
        get
        {
            return viewRange;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }
    }
    #endregion

    /*
    MEMORY DUMP:
    Dont use arrays but separate fields for distinct elements of the same type (Attributes) because of the unity inspector
    */

}
