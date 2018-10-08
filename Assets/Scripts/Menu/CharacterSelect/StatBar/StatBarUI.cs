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

    protected override void LoadDataInternal()
    {
        IServerData server = Main.ServerData;

        UnitAttributeStats stats = server.GetAttributeStats(statType);
        HeroData heroData = Main.GameState.CurrentHeroData;

        statNameText.text = statName;
        
        if (heroData != null && stats != null)
        {
            float statValue = heroData.GetStat(statType);
            slider.value = stats.GetSliderValue(heroData.Level, statValue);
            statValueText.text = statValue.ToString();
        }
        else
        {
            slider.value = 0;
            statValueText.text = "?";
        }
    }

}
