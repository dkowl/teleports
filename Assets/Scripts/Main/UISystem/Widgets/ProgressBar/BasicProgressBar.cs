﻿using System.Collections;
using System.Collections.Generic;
using Teleports.Utils;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BasicProgressBar : MonoBehaviour {

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

    public enum ValueInterpreterType
    {
        Basic,
        XP,
        RP
    }

    public struct Values
    {
        public float current;
        public float target;
        public float min;
        public float max;
        public float delta;
    }

    [SerializeField] private GameObject prefab;
    [SerializeField] private Slider slider;
    [SerializeField] private Text nameText;
    [SerializeField] private Text valueText;
    [SerializeField] private int secondaryTextNo;
    [SerializeField] private Text[] secondaryTexts;
    [SerializeField] private Values values;
    [SerializeField] private ValueTextType valueTextType;
    [SerializeField] private AnimationType animationType;
    [SerializeField] private ValueInterpreterType valueInterpreterType;
    [SerializeField] private float linearAnimationSpeed = 0.01f;
    [SerializeField] private float asymptoticAnimationSpeed = 0.05f;

    private PrefabSpawner prefabSpawner;

    [Button]
    private void Awake()
    {
        prefabSpawner = gameObject.GetOrAddComponent<PrefabSpawner>();
        prefabSpawner.Prefab = prefab;
        prefabSpawner.AfterSpawnAction = ChildrenSetup;
        prefabSpawner.Respawn();
    }

    [Button]
    private void Update()
    {
        values.current = GetNextValue();
        UpdateUiElements();
    }

    public void SetValues(Values values)
    {
        Debug.Assert(values.min < values.max);
        Debug.Assert(values.current <= values.max && values.current >= values.min);
        Debug.Assert(values.target <= values.max && values.target >= values.min);

        this.values = values;

        UpdateUiElements();
    }

    private float GetNextValue()
    {
        float nextValue = values.current;
        int direction = values.current < values.target ? 1 : -1;

        if (animationType == AnimationType.None)
        {
            return values.target;
        }

        if (animationType == AnimationType.Linear || animationType == AnimationType.Asymptotic)
        {
            float delta = linearAnimationSpeed * ValueRange;
            float maxAllowedDelta = Mathf.Abs(values.target - values.current);
            if (delta > maxAllowedDelta)
            {
                delta = maxAllowedDelta;
            }
            nextValue += delta * direction;
        }

        if (animationType == AnimationType.Asymptotic)
        {
            nextValue = Mathf.Clamp
            (
                nextValue +
                asymptoticAnimationSpeed * (values.target - nextValue),
                0,
                values.max + 1
            );
        }

        return nextValue;
    }

    private void ChildrenSetup()
    {
        this.FindOrSpawnChildWithComponent(ref slider, "Slider");
        this.FindOrSpawnChildWithComponent(ref nameText, "NameText", true);
        this.FindOrSpawnChildWithComponent(ref valueText, "ValueText", true);
        secondaryTexts = new TextMeshProUGUI[secondaryTextNo];
        for (int i = 0; i < secondaryTextNo; i++)
        {
            this.FindOrSpawnChildWithComponent(ref secondaryTexts[i], "SecondaryText" + i.ToString(), true);
        }
    }

    private IProgressBarValueInterpreter CreateValueInterpreter()
    {
        IProgressBarValueInterpreter result;
        switch (valueInterpreterType)
        {
            case ValueInterpreterType.Basic:
                result = new ProgressBarValueInterpreter();
                break;
            case ValueInterpreterType.XP:
                result = new ProgressBarValueInterpreter_XP();
                break;
            case ValueInterpreterType.RP:
                result = new ProgressBarValueInterpreter_RP();
                break;
            default:
                result = new ProgressBarValueInterpreter();
                break;
        }
        result.SetValues(values.current, values.target, values.min, values.max, values.delta);
        result.SetValueTextType(valueTextType);

        return result;
    }

    private void UpdateUiElements()
    {
        IProgressBarValueInterpreter valueInterpreter = CreateValueInterpreter();
        if (nameText != null) nameText.text = valueInterpreter.NameTextString();
        if (valueText != null) valueText.text = valueInterpreter.ValueTextString();
        for (int i = 0; i < secondaryTextNo; i++)
        {
            if (secondaryTexts[i] != null) secondaryTexts[i].text = valueInterpreter.SecondaryTextString(i);
        }
        slider.value = valueInterpreter.SliderValue();
    }

    public bool IsAnimating => values.current != values.target;
    private float ValueRange => values.max - values.min;
}