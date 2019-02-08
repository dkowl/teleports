using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stylesheet", menuName = "Custom/Stylesheet", order = 7)]
public class Stylesheet_Legacy : ScriptableObject
{
    public enum FontSize
    {
        Title,
        Big,
        MediumBig,
        Medium,
        MediumSmall,
        Small,
        WorldFloating
    }

    public enum TextColor
    {
        Default,
        WorldUI,
        HomeMenu,
        InputFieldPlaceholder
    }

    public enum ColorPreset
    {
        Gray1,
        Gray2,
        Gray3,
        Gray4,
        Gray5,
        Black,
        Transparent,
        Alpha80,
        White,
        Background
    }

    public float titleFontSize;
    public float bigFontSize;
    public float mediumBigFontSize;
    public float mediumFontSize;
    public float mediumSmallFontSize;
    public float smallFontSize;
    public float worldFloatingFontSize;

    public Color defaultTextColor;
    public Color worldUiTextColor;
    public Color homeMenuTextColor = Color.white;

    public Color gray1;
    public Color gray2;
    public Color gray3;
    public Color gray4;
    public Color gray5;
    public Color black;
    public Color backgroundColor;

    public Sprite lockSprite;

    public float GetFontSize(FontSize fontSize)
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
            case FontSize.WorldFloating:
                return worldFloatingFontSize;
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
            case TextColor.WorldUI:
                return worldUiTextColor;
            case TextColor.HomeMenu:
                return homeMenuTextColor;
            case TextColor.InputFieldPlaceholder:
                Color result = homeMenuTextColor;
                result.a = 0.25f;
                return result;
            default:
                return defaultTextColor;
        }
    }

    public Color GetColorPreset(ColorPreset colorPreset)
    {
        switch (colorPreset)
        {
            case ColorPreset.White:
                return Color.white;
            case ColorPreset.Gray1:
                return gray1;
            case ColorPreset.Gray2:
                return gray2;
            case ColorPreset.Gray3:
                return gray3;
            case ColorPreset.Gray4:
                return gray4;
            case ColorPreset.Gray5:
                return gray5;
            case ColorPreset.Black:
                return black;
            case ColorPreset.Transparent:
                return Color.clear;
            case ColorPreset.Alpha80:
                return new Color(0, 0, 0, 0.8f);
            case ColorPreset.Background:
                return backgroundColor;
            default:
                return Color.white;
        }
    }
}
