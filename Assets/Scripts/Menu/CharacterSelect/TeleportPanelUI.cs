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

    public override void LoadDataInternal()
    {
        ownerNameText.text = MainData.CurrentPlayerData.CharacterName + "'s";
        tierText.text = "Tier " + RomanNumbers.RomanNumber(MainData.CurrentPlayerData.CurrentTeleportData.Tier);
        rankPointsText.text = MainData.CurrentPlayerData.RankPoints.ToString();
    }
}
