using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ValueTextType = BasicProgressBar.ValueTextType;

public class ProgressBarValueInterpreter : IProgressBarValueInterpreter
{
    protected BasicProgressBar.Values values;

    protected ValueTextType valueTextType;

    public void SetValues(BasicProgressBar.Values values)
    {
        this.values = values;
    }

    public void SetValueTextType(ValueTextType valueTextType)
    {
        this.valueTextType = valueTextType;
    }

    public virtual BasicProgressBar.Values InterpretValues(float current, float target)
    {
        var valuesCopy = values;
        valuesCopy.current = current;
        valuesCopy.target = target;
        return valuesCopy;
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
                return (values.current - values.min).ToString("F0");
            case ValueTextType.TwoValues:
                return (values.current - values.min).ToString("F0") + " / " + (values.max - values.min).ToString("F0");
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
        return Mathf.Clamp((values.current - values.min) / (values.max - values.min), 0f, 1f);
    }

    protected string DeltaString
    {
        get
        {
            if (values.delta > 0)
                return '+' + values.delta.ToString();
            else if (values.delta < 0)
                return values.delta.ToString();
            else
            {
                return values.delta.ToString();
            }
        }
    }
}

