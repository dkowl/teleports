using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageStyler : LoadableBehaviour {

    public Stylesheet_Legacy.ColorPreset color;

    protected override void LoadDataInternal()
    {
        Image image = gameObject.GetComponent<Image>();
        image.color = Main.StaticData.StylesheetLegacy.GetColorPreset(color);
    }
}
