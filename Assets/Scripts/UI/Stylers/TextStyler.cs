using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextStyler : LoadableBehaviour {

    public Stylesheet.FontSize fontSize;
    public Stylesheet.TextColor textColor;

	public override void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;
        Text text = gameObject.GetComponent<Text>();

        text.fontSize = stylesheet.GetFontSize(fontSize);
        text.color = stylesheet.GetTextColor(textColor);  
    }
}
