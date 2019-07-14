using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWidgetStyle : ITextWidgetStyle {

	[SerializeField] private TMPro.TMP_FontAsset font;
    [SerializeField] private StylesheetKeys.FontSize fontSize;

    public TMPro.TMP_FontAsset Font => font;
    public int? FontSize => fontSize != null ? (int?)Main.StaticData.UI.Stylesheet.GetValue<float>(fontSize) : null;
}
