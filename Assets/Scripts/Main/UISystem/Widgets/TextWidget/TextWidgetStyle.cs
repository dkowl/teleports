using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWidgetStyle : ITextWidgetStyle {

	[SerializeField] private TMPro.TMP_FontAsset font;

    public TMPro.TMP_FontAsset Font => font;
}
