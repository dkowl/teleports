using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using System;

public class InventorySlotUI : LoadableBehaviour {

    InventorySlot slotData;

    RawImage itemIcon;
    Text counterText;

    protected override void LoadDataInternal()
    {
        throw new NotImplementedException();
    }
}
