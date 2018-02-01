using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageStyler : LoadableBehaviour {

    public Stylesheet.ColorPreset color;

    override protected void LoadDataInternal()
    {
        Image image = gameObject.GetComponent<Image>();
        image.color = MainData.Stylesheet.GetColorPreset(color);
    }
}
