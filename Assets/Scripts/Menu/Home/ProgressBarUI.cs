using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ProgressBarUI : MenuBehaviour {

    public enum ValueTextType
    {
        None,
        OneValue,
        TwoValues,
        Percentage
    }

    public enum AnimationType
    {
        None,
        Asymptotic,
        Custom
    }

    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI valueText;
    [SerializeField] protected float currentValue;
    [SerializeField] protected float maxValue;
    [SerializeField] protected ValueTextType valueTextType;
    [SerializeField] protected AnimationType animationType;

    protected override void OnLoadInternal()
    {
        if (!DetectChange())
        {
            LoadFinish();
            return;
        }
        else
        {
            slider.value = currentValue/maxValue;
            base.OnLoadInternal();
        }
    }

    protected virtual bool DetectChange()
    {
        bool result = false;
        float nextCurrentValue = CurrentValue();
        float nextMaxValue = MaxValue();
        if(currentValue != nextCurrentValue)
        {
            currentValue = nextCurrentValue;
            result = true;
        }
        if(maxValue != nextMaxValue)
        {
            maxValue = nextMaxValue;
            result = true;
        }
        return result;
    }

    protected virtual string ValueTextString()
    {
        switch (valueTextType)
        {
            case ValueTextType.None:
                return "";
            case ValueTextType.OneValue:
                return currentValue.ToString("D");
            case ValueTextType.TwoValues:
                return currentValue.ToString("D") + "/" + maxValue.ToString("D");
            case ValueTextType.Percentage:
                return (currentValue / maxValue).ToString("P0");
            default:
                return "";
        }
    }

    protected abstract float CurrentValue();
    protected abstract float MaxValue();



}
