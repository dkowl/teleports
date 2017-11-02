using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using Teleports.Utils;

public abstract class BaseProgressBarUI : MenuBehaviour {

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
        Linear,
        Asymptotic,
        Custom
    }

    private float displayValue;

    [SerializeField] protected Slider slider;
    [SerializeField] protected Text nameText;
    [SerializeField] protected Text valueText;
    [SerializeField] protected int secondaryTextNo;
    [SerializeField] protected Text[] secondaryTexts;
    [SerializeField] protected float currentValue;
    [SerializeField] protected float maxValue;
    [SerializeField] protected ValueTextType valueTextType;
    [SerializeField] protected AnimationType animationType;
    [SerializeField] protected float linearAnimationSpeed = 0.01f;
    [SerializeField] protected float asymptoticAnimationSpeed = 0.05f;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.FindOrSpawnChildWithComponent(ref slider, "Slider");
        this.FindOrSpawnChildWithComponent(ref nameText, "NameText", true);
        this.FindOrSpawnChildWithComponent(ref valueText, "ValueText", true);
        secondaryTexts = new Text[secondaryTextNo];
        for (int i = 0; i<secondaryTextNo; i++)
        {
            this.FindOrSpawnChildWithComponent(ref secondaryTexts[i], "SecondaryText" + i.ToString(), true);
        }
        SkipAnimation();
    }

    void Update()
    {
        if (displayValue < MinValue() || displayValue > maxValue)
            SkipAnimation();
        else
            Animate(DisplayValue, currentValue, animationType);

        if (nameText != null) nameText.text = NameTextString();
        if (valueText != null) valueText.text = ValueTextString();
        for (int i = 0; i < secondaryTextNo; i++)
        {
            if(secondaryTexts[i] != null) secondaryTexts[i].text = SecondaryTextString(i);
        }
    }

    protected override void OnLoadInternal()
    {
        if (!DetectChange())
        {
            LoadFinish();
            return;
        }
        else
        {
            base.OnLoadInternal();
        }
    }

    public float Animate(float disp, float cur, AnimationType type)
    {
        int direction;
        if (disp < cur) direction = 1;
        else direction = -1;
        switch (type)
        {
            case AnimationType.None:
                disp = cur;
                break;
            case AnimationType.Linear:
                disp = disp + linearAnimationSpeed * maxValue * direction;
                if(direction == 1)
                {
                    disp = Mathf.Clamp(disp, 0, cur);
                }
                else
                {
                    disp = Mathf.Clamp(disp, cur, maxValue);
                }
                break;
            case AnimationType.Asymptotic:
                disp = Animate(disp, cur, AnimationType.Linear);
                disp = Mathf.Clamp
                    (
                    disp +
                    asymptoticAnimationSpeed * (cur - disp),
                    0,
                    maxValue + 1
                    );
                break;
        }
        DisplayValue = disp;
        return disp;
    }

    public void SkipAnimation()
    {
        Animate(displayValue, currentValue, AnimationType.None);
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

    protected virtual string NameTextString()
    {
        return "";
    }

    protected virtual string ValueTextString()
    {
        switch (valueTextType)
        {
            case ValueTextType.None:
                return "";
            case ValueTextType.OneValue:
                return (displayValue - MinValue()).ToString("F0");
            case ValueTextType.TwoValues:
                return (displayValue - MinValue()).ToString("F0") + " / " + (maxValue - MinValue()).ToString("F0");
            case ValueTextType.Percentage:
                return slider.value.ToString("P0");
            default:
                return "";
        }
    }

    protected virtual string SecondaryTextString(int id)
    {
        return "";
    }

    protected virtual float SliderValue()
    {
        return Mathf.Clamp(displayValue / maxValue, 0f, 1f);
    }

    protected abstract float CurrentValue();
    protected abstract float MaxValue();

    protected virtual float MinValue()
    {
        return 0;
    }

    protected float DisplayValue
    {
        get { return displayValue; }
        set
        {
            displayValue = value;
            slider.value = SliderValue();
        }
    }

}
