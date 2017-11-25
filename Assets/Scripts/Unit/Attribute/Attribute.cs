using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Attribute
{
    [FormerlySerializedAs("raw_")] [SerializeField] private float raw;
    [FormerlySerializedAs("bonus_")] [SerializeField] private float bonus;
    [FormerlySerializedAs("multiplier_")] [SerializeField] private float multiplier;

    public Attribute() : this(0)
    {
    }

    public Attribute(float raw)
    {
        this.raw = raw;
        bonus = 0;
        multiplier = 1;
    }

    public void AddBonus(float bonus)
    {
        this.bonus += bonus;
    }

    public void AddMultiplier(float multiplier)
    {
        this.multiplier *= multiplier;
    }

    public void Modify(float bonus, float multiplier)
    {
        AddBonus(bonus);
        AddMultiplier(multiplier);
    }

    public float Value
    {
        get { return (raw + bonus) * multiplier; }
    }
};
