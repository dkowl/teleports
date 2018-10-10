using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

public class TeleportPanelUI : LoadableBehaviour {

    public Text ownerNameText;
    public Text tierText;
    public Text rankPointsText;

    protected override void LoadDataInternal()
    {
        HeroData heroData = Main.GameState.CurrentHeroData;
        if (heroData != null)
        {
            ownerNameText.text = heroData.CharacterName + "'s";
            tierText.text = "Tier " + RomanNumbers.RomanNumber(heroData.TeleportData.Tier);
            rankPointsText.text = heroData.RankPoints.ToString();
        }
        else
        {
            ownerNameText.text = "Empty";
            tierText.text = "";
            rankPointsText.text = "";
        }
    }
}
