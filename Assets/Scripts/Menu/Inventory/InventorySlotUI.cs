using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using System;

public class InventorySlotUI : LoadableBehaviour {

    int id;
    InventorySlotData slotData;

    RawImage itemIcon;
    Text counterText;

    protected override void LoadDataInternal()
    {
        
    }
}
