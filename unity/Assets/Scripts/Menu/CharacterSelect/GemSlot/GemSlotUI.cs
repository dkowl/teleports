using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlotUI : UnlockableSlotUI {

    [SerializeField]
    private int gemSlotID;

    private GemSlot gemSlot;

    protected override UnlockableSlot GetSlot()
    {
        var heroData = Main.GameState.CurrentHeroData;
        if(heroData != null)
        {
            gemSlot = heroData.TeleportData.GetGemSlot(gemSlotID);
            return gemSlot;
        }
        else
        {
            return new UnlockableSlot();
        }
    }

    protected override void OnFull()
    {
        base.OnFull();
        text.text = gemSlot.Essence.ToString();
    }

    public void SetSlotID(int id)
    {
        gemSlotID = id;
    }
}
