using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStylesheet
{
    float GetFontSize(string key);
    Color GetTextColor(string key);
    Color GetColor(string key);
}
