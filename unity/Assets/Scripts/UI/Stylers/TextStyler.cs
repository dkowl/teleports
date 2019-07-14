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
