using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public float raw_;
    public float bonus_, multiplier_;

    public Attribute()
    {
        bonus_ = 0;
        multiplier_ = 1;
    }

    public void addBonus(float bonus)
    {
        bonus_ += bonus;
    }

    public void addMultiplier(float multiplier)
    {
        multiplier_ *= multiplier;
    }

    public void modify(float bonus, float multiplier)
    {
        addBonus(bonus);
        addMultiplier(multiplier);
    }

    public float value()
    {
        return (raw_ + bonus_) * multiplier_;
    }
};
