using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageStyler : LoadableBehaviour {
    
    [SerializeField] private StylesheetKeys.Color colorPreset;

    protected override void LoadDataInternal()
    {
        Image image = gameObject.GetComponent<Image>();
        image.color = Main.StaticData.UI.Stylesheet.GetValue<Color>(colorPreset);
    }
}
