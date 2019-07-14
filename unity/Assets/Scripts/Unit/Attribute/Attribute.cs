using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

[System.Serializable]
public class Attribute
{
    [FormerlySerializedAs("raw_"), SerializeField, HideLabel, LabelWidth(15), HorizontalGroup("1")] private float raw;
    [FormerlySerializedAs("bonus_"), SerializeField, LabelWidth(15), LabelText("+"), HorizontalGroup("1")] private float bonus;
    [FormerlySerializedAs("multiplier_"), SerializeField, LabelWidth(15), LabelText("*"), HorizontalGroup("1")] private float multiplier = 1;

    public Attribute() : this(0)
    {
    }

    public Attribute(Attribute other)
    {
        raw = other.raw;
        bonus = other.bonus;
        multiplier = other.multiplier;
    }

    public Attribute(float raw)
    {
        Reset(raw);
    }

    public void AddBase(float bonus)
    {
        raw += bonus;
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

    public void Reset(float raw)
    {
        this.raw = raw;
        bonus = 0;
        multiplier = 1;
    }

    [ShowInInspector, HideLabel, LabelWidth(15), HorizontalGroup("1")]
    public float Value
    {
        get { return (raw + bonus) * multiplier; }
    }

    public float BaseValue => raw;
};
