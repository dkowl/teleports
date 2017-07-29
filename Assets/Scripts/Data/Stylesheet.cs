using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stylesheet", menuName = "Custom/Stylesheet", order = 7)]
public class Stylesheet : ScriptableObject
{
    public enum FontSize
    {
        Title,
        Big,
        MediumBig,
        Medium,
        MediumSmall,
        Small
    }

    public enum TextColor
    {
        Default
    }

    public int titleFontSize;
    public int bigFontSize;
    public int mediumBigFontSize;
    public int mediumFontSize;
    public int mediumSmallFontSize;
    public int smallFontSize;
    public Color defaultTextColor;

    public int GetFontSize(FontSize fontSize)
    {
        switch (fontSize)
        {
            case FontSize.Title:
                return titleFontSize;
            case FontSize.Big:
                return bigFontSize;
            case FontSize.MediumBig:
                return mediumBigFontSize;
            case FontSize.Medium:
                return mediumFontSize;
            case FontSize.MediumSmall:
                return mediumSmallFontSize;
            case FontSize.Small:
                return smallFontSize;
            default:
                return titleFontSize;
        }
    }

    public Color GetTextColor(TextColor textColor)
    {
        switch (textColor)
        {
            case TextColor.Default:
                return defaultTextColor;
            default:
                return defaultTextColor;
        }
    }
}
