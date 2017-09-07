using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : LoadableBehaviour {

    public PlayerStats statType;
    public string statName;
    public Text statNameText, statValueText;
    public Slider slider;

    public override void LoadData()
    {
        IServerData server = MainData.CurrentServerData;

        UnitAttributeStats stats = server.GetAttributeStats(statType);


    }

}
