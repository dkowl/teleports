using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class TextStyler : LoadableBehaviour {

    [SerializeField] private Stylesheet_Legacy.FontSize fontSize;
    [SerializeField] private Stylesheet_Legacy.TextColor textColor;
    [SerializeField] private StylesheetKeys.FontSize fontSizee;
    [SerializeField, Range(0, 1)] private float alphaMultiplier = 1f;

    protected override void LoadDataInternal()
    {
        Stylesheet_Legacy stylesheet = Main.StaticData.StylesheetLegacy;
        int i = 0;

        Text text = gameObject.GetComponent<Text>();
        if (text != null)
        {
            text.fontSize = (int)stylesheet.GetFontSize(fontSize);
            text.color = GetTextColor();
        }

        TMPro.TextMeshProUGUI tmpText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        if(tmpText != null)
        {
            tmpText.fontSize = stylesheet.GetFontSize(fontSize);
            tmpText.color = GetTextColor();
        }
    }

    private Color GetTextColor()
    {
        Stylesheet_Legacy stylesheet = Main.StaticData.StylesheetLegacy;
        Color result = stylesheet.GetTextColor(textColor);
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
