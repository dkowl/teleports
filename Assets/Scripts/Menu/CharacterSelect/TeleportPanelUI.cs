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

    override protected void LoadDataInternal()
    {
        IPlayerData playerData = MainData.CurrentPlayerData;
        if (playerData != null)
        {
            ownerNameText.text = playerData.CharacterName + "'s";
            tierText.text = "Tier " + RomanNumbers.RomanNumber(playerData.CurrentTeleportData.Tier);
            rankPointsText.text = playerData.RankPoints.ToString();
        }
        else
        {
            ownerNameText.text = "Empty";
            tierText.text = "";
            rankPointsText.text = "";
        }
    }
}
