using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextStyler : MonoBehaviour {

    public Stylesheet.FontSize fontSize;
    public Stylesheet.TextColor textColor;

	void OnEnable()
    {
        if (Application.isEditor)
        {
            Stylesheet stylesheet = MainData.CurrentStylesheet;
            Text text = gameObject.GetComponent<Text>();

            text.fontSize = stylesheet.GetFontSize(fontSize);
            text.color = stylesheet.GetTextColor(textColor);
        }

    }
}
