using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public float raw;
    public float bonus_, modifier_;

    public Attribute()
    {
        bonus_ = 0;
        modifier_ = 1;
    }

    public void addBonus(float bonus)
    {
        bonus_ += bonus;
    }

    public void addModifier(float modifier)
    {
        modifier_ *= modifier;
    }

    public float value()
    {
        return (raw + bonus_) * modifier_;
    }
};
