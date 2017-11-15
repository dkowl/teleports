using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class ProgressBarUI : BaseProgressBarUI, IMessageHandler<RunFinishedMessage> {

    public enum ValueType
    {
        XP,
        RP
    }

    private float delta;

    [SerializeField] protected ValueType valueType;

    protected override void Awake()
    {
        base.Awake();
        MainData.MessageBus.Subscribe(this);
    }

    protected override bool DetectChange()
    {
        if (MainData.CurrentPlayerData == null) return false;
        return base.DetectChange();
    }

    protected override void OnChangeDetected()
    {
        delta = currentValue - DisplayValue;
    }

    protected override string NameTextString()
    {
        switch (valueType)
        {
            case ValueType.XP:
                return "Level " + CurrentLevels.Level((int)DisplayValue).ToString();
            case ValueType.RP:
                return "Rank " + RomanNumbers.RomanNumber(CurrentLevels.Level((int)DisplayValue));
        }
        return "Name";
    }

    protected override string ValueTextString()
    {
        if (valueType == ValueType.XP && CurrentLevels.Level((int)DisplayValue) == CurrentLevels.MaxLevel)
        {
            return "Max";
        }
        else
        {
            return base.ValueTextString();
        }
    }

    protected override string SecondaryTextString(int id)
    {
        if (valueType == ValueType.RP)
        {
            switch (id)
            {
                case 0:
                    return CurrentLevels.Owned((int)DisplayValue).ToString();
                case 1:
                    return (CurrentLevels.Owned((int)DisplayValue) + CurrentLevels.Required((int)DisplayValue)).ToString();
                case 2:
                    return DeltaString;
                default:
                    return "";
            }
        }
        if(valueType == ValueType.XP)
        {
            switch (id)
            {
                case 2:
                    return DeltaString;
                default:
                    return "";
            }
        }
        else return "";
    }

    protected override float CurrentValue()
    {
        switch (valueType)
        {
            case ValueType.XP:
                return MainData.CurrentPlayerData.Xp;
            case ValueType.RP:
                return MainData.CurrentPlayerData.RankPoints;
            default:
                return 0;
        }
    }

    protected override float MaxValue()
    {
        int disp = (int)DisplayValue;
        return Mathf.Min(disp + CurrentLevels.Required(disp) - CurrentLevels.Current(disp), CurrentLevels.MaxValue);
    }

    protected override float MinValue()
    {
        if(valueType == ValueType.XP)
        {
            return CurrentLevels.Owned((int)DisplayValue);
        }
        else
        {
            return base.MinValue();
        }
    }

    protected override float SliderValue()
    {
        if (valueType == ValueType.XP || valueType == ValueType.RP)
        {
            maxValue = MaxValue();
            return Mathf.Clamp(CurrentLevels.Progress((int)DisplayValue), 0f, 1f);
        }
        else
        {
            return base.SliderValue();
        }
    }

    public void Handle(RunFinishedMessage message)
    {
        AnimateNextChange();
    }

    protected string DeltaString
    {
        get {
            if (delta > 0)
                return '+' + delta.ToString();
            else
                return delta.ToString();
        }
    }

    protected Levels CurrentLevels
    {
        get
        {
            switch (valueType)
            {
                case ValueType.XP:
                    return Levels.xp;
                case ValueType.RP:
                    return Levels.rp;
                default:
                    return Levels.xp;
            }
        }
    }
}
