using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressBarValueInterpreter
{
    BasicProgressBar.Values InterpretValues(float current, float target);
    void SetValues(BasicProgressBar.Values values);

    void SetValueTextType(BasicProgressBar.ValueTextType valueTextType);

    string NameTextString();
    string ValueTextString();
    string SecondaryTextString(int id);
    float SliderValue();
}
