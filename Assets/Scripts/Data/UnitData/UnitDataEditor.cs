using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "unitData", menuName = "Custom/UnitData", order = 1)]
public class UnitDataEditor : ScriptableObject
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

    #region properties
    public string Name
    {
        get
        {
            return unitName;
        }
        set
        {
            unitName = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public float Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
        }
    }

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public float Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    public float Regen
    {
        get
        {
            return regen;
        }
        set
        {
            regen = value;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float ArmorIgnore
    {
        get
        {
            return armorIgnore;
        }
        set
        {
            armorIgnore = value;
        }
    }

    public float Reach
    {
        get
        {
            return reach;
        }
        set
        {
            reach = value;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    public float ViewRange
    {
        get
        {
            return viewRange;
        }
        set
        {
            viewRange = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
        }
    }
    #endregion
}
