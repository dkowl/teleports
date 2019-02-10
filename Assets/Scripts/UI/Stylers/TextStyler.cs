using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class TextStyler : LoadableBehaviour {
    
    [SerializeField] private StylesheetKeys.FontSize fontSizePreset;
    [SerializeField] private StylesheetKeys.TextColor textColorPreset;
    [SerializeField, Range(0, 1)] private float alphaMultiplier = 1f;

    public string FontSizeToString(Stylesheet_Legacy.FontSize fontSize)
    {
        switch (fontSize)
        {
            case Stylesheet_Legacy.FontSize.Big:
                return "Big";
            case Stylesheet_Legacy.FontSize.Medium:
                return "Medium";
            case Stylesheet_Legacy.FontSize.MediumBig:
                return "Medium Big";
            case Stylesheet_Legacy.FontSize.MediumSmall:
                return "Medium Small";
            case Stylesheet_Legacy.FontSize.Small:
                return "Small";
            case Stylesheet_Legacy.FontSize.Title:
                return "Title";
        }
        Debug.LogError("Invalid FontSize enum");
        return null;
    }

    public string TextColorToString(Stylesheet_Legacy.TextColor textColor)
    {
        switch (textColor)
        {
            case Stylesheet_Legacy.TextColor.Default:
                return "Default";
            case Stylesheet_Legacy.TextColor.HomeMenu:
                return "Home Menu";
            case Stylesheet_Legacy.TextColor.InputFieldPlaceholder:
                return "Input Field Placeholder";
            case Stylesheet_Legacy.TextColor.WorldUI:
                return "World UI";
        }
        Debug.LogError("Invalid TextColor enum");
        return null;
    }

    protected override void LoadDataInternal()
    {
        IStylesheet stylesheet = Main.StaticData.UI.Stylesheet;
        int i = 0;

        Text text = gameObject.GetComponent<Text>();
        if (text != null)
        {
            text.fontSize = (int)stylesheet.GetValue<float>(fontSizePreset);
            text.color = GetTextColor();
        }

        TMPro.TextMeshProUGUI tmpText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        if(tmpText != null)
        {
            tmpText.fontSize = stylesheet.GetValue<float>(fontSizePreset);
            tmpText.color = GetTextColor();
        }
    }

    private Color GetTextColor()
    {
        IStylesheet stylesheet = Main.StaticData.UI.Stylesheet;
        Color result = stylesheet.GetValue<Color>(textColorPreset);
        result.a *= alphaMultiplier;
        return result;
    }

    public float AlphaMultiplier
    {
        get { return alphaMultiplier; }
        set
        {
            alphaMultiplier = value;
            LoadData();
        }
    }
}
