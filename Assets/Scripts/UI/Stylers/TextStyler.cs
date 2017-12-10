using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextStyler : LoadableBehaviour {

    public Stylesheet.FontSize fontSize;
    public Stylesheet.TextColor textColor;

    override protected void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;

        Text text = gameObject.GetComponent<Text>();
        if (text != null)
        {
            text.fontSize = stylesheet.GetFontSize(fontSize);
            text.color = stylesheet.GetTextColor(textColor);
        }

        TMPro.TextMeshProUGUI tmpText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        if(tmpText != null)
        {
            tmpText.fontSize = stylesheet.GetFontSize(fontSize);
            tmpText.color = stylesheet.GetTextColor(textColor);
        }
    }
}
