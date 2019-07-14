using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWidgetStylePreset : ITextWidgetStyle {

    [SerializeField] StylesheetKeys.TextStyle stylePreset = new StylesheetKeys.TextStyle();

    private TextWidgetStyle Style => Main.StaticData.UI.Stylesheet.GetValue<TextWidgetStyle>(stylePreset);
    public TMPro.TMP_FontAsset Font => Style.Font;
    public int? FontSize => Style.FontSize;
}
