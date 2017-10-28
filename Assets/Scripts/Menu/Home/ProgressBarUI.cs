using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarUI : BaseProgressBarUI {

    public enum ValueType
    {
        XP,
        RP
    }

    [SerializeField] protected ValueType valueType;

    protected override string NameTextString()
    {
        return "Level " + Levels.xp.Level((int)DisplayValue).ToString();
    }

    protected override float CurrentValue()
    {
        switch (valueType)
        {
            case ValueType.XP:
                return MainData.CurrentPlayerData.Xp;
            case ValueType.RP:
                return 0;
            default:
                return 0;
        }
    }

    protected override float MaxValue()
    {
        int xp = (int)DisplayValue;
        return xp + Levels.xp.Required(xp) - Levels.xp.Current(xp);
    }

    protected override float SliderValue()
    {
        if (valueType == ValueType.XP)
        {
            maxValue = MaxValue();
            return Mathf.Clamp(Levels.xp.Progress((int)DisplayValue), 0f, 1f);
        }
        else
        {
            return base.SliderValue();
        }
    }
}
