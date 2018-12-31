using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressBarValueInterpreter
{
    void SetValues(
        float currentValue,
        float targetValue,
        float minValue,
        float maxValue,
        float delta);

    void SetValueTextType(BasicProgressBar.ValueTextType valueTextType);

    string NameTextString();
    string ValueTextString();
    string SecondaryTextString(int id);
    float SliderValue();
}
