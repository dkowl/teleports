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

    [SerializeField] private float displayValue;

    private PrefabSpawner spawner;
    private bool firstStart = true;
    private float currentFreezeTime = 0;

    [SerializeField] protected GameObject prefab;
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
    [SerializeField] protected float freezeTime = 0;
    [SerializeField] protected bool detectChangeInUpdate = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        SaveDataSO.LoadEvent -= SkipNextAnimation;
        SaveDataSO.LoadEvent += SkipNextAnimation;

        spawner = gameObject.GetComponent<PrefabSpawner>();
        if(spawner == null)
        {
            spawner = gameObject.AddComponent<PrefabSpawner>();
        }
        spawner.prefab = prefab;
        spawner.Spawn();

        this.FindOrSpawnChildWithComponent(ref slider, "Slider");
        this.FindOrSpawnChildWithComponent(ref nameText, "NameText", true);
        this.FindOrSpawnChildWithComponent(ref valueText, "ValueText", true);
        secondaryTexts = new Text[secondaryTextNo];
        for (int i = 0; i<secondaryTextNo; i++)
        {
            this.FindOrSpawnChildWithComponent(ref secondaryTexts[i], "SecondaryText" + i.ToString(), true);
        }
    }

    void Update()
    {
        if (detectChangeInUpdate)
            DetectChange();

        if (displayValue < MinValue() || displayValue > maxValue)
            SkipAnimation();
        else if(currentFreezeTime > 0)
            currentFreezeTime -= Time.deltaTime;
        else
            Animate(DisplayValue, currentValue, animationType);

        if (nameText != null) nameText.text = NameTextString();
        if (valueText != null) valueText.text = ValueTextString();
        for (int i = 0; i < secondaryTextNo; i++)
        {
            if(secondaryTexts[i] != null) secondaryTexts[i].text = SecondaryTextString(i);
        }
        slider.value = SliderValue();
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
        Debug.Log("Skipping animation!");
        do
        {
            Animate(displayValue, currentValue, AnimationType.None);
        } while (DetectChange());
        Skip();
    }

    public void Freeze(float time)
    {
        currentFreezeTime = Mathf.Max(time, currentFreezeTime);
    }

    public void Freeze()
    {
        Freeze(freezeTime);
    }

    public void SkipNextAnimation()
    {
        if (firstStart) return;
        else
        {
            firstStart = true;
            if (!DetectChange())
            {
                firstStart = false;
            }
        }
    }

    protected override bool DetectChange()
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
        if (result)
        {
            Debug.Log("Change detected! New currentValue: " + currentValue.ToString() + " New maxValue: " + maxValue.ToString());
            if (firstStart)
            {
                SkipAnimation();
                firstStart = false;
                return false;
            }
            else
            {
                Freeze();
            }
            OnChangeDetected();
        }
        return result;
    }

    protected virtual void OnChangeDetected()
    {

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
        }
    }

}
