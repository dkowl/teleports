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

    public override void LoadDataInternal()
    {
        IServerData server = MainData.CurrentServerData;

        UnitAttributeStats stats = server.GetAttributeStats(statType);
        IPlayerData playerData = MainData.CurrentPlayerData;

        statNameText.text = statName;
        
        if (playerData != null)
        {
            float statValue = playerData.GetStat(statType);
            slider.value = stats.GetSliderValue(playerData.Level, statValue);
            statValueText.text = statValue.ToString();
        }
        else
        {
            slider.value = 0;
            statValueText.text = "?";
        }
    }

}
