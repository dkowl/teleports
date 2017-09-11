using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageStyler : LoadableBehaviour {

    public Stylesheet.ColorPreset color;

    public override void LoadDataInternal()
    {
        Image image = gameObject.GetComponent<Image>();
        image.color = MainData.CurrentStylesheet.GetColorPreset(color);
    }
}
