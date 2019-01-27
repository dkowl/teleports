using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ValueTextType = BasicProgressBar.ValueTextType;

public class ProgressBarValueInterpreter : IProgressBarValueInterpreter
{
    protected float currentValue;
    protected float targetValue;
    protected float minValue;
    protected float maxValue;
    protected float delta;

    protected ValueTextType valueTextType;

    public void SetValues(
        float currentValue,
        float targetValue,
        float minValue,
        float maxValue,
        float delta)
    {
        this.currentValue = currentValue;
        this.targetValue = targetValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.delta = delta;
    }

    public void SetValueTextType(ValueTextType valueTextType)
    {
        this.valueTextType = valueTextType;
    }

    public virtual string NameTextString()
    {
        return "";
    }

    public virtual string ValueTextString()
    {
        switch (valueTextType)
        {
            case ValueTextType.None:
                return "";
            case ValueTextType.OneValue:
                return (currentValue - minValue).ToString("F0");
            case ValueTextType.TwoValues:
                return (currentValue - minValue).ToString("F0") + " / " + (maxValue - minValue).ToString("F0");
            case ValueTextType.Percentage:
                return SliderValue().ToString("P0");
            default:
                return "";
        }
    }

    public virtual string SecondaryTextString(int id)
    {
        return id == 2 ? DeltaString : "";
    }

    public virtual float SliderValue()
    {
        return Mathf.Clamp((currentValue - minValue) / maxValue, 0f, 1f);
    }

    protected string DeltaString
    {
        get
        {
            if (delta > 0)
                return '+' + delta.ToString();
            else if (delta < 0)
                return delta.ToString();
            else
            {
                return delta.ToString();
            }
        }
    }
}

