using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerGraphics {

	public static Sprite GetPlayerIcon(HeroData heroData)
    {
        return Main.StaticData.Game.Races.GetValue(heroData.RaceId).Graphics.Icon;
    }

    public static Sprite GetTeleportIcon(HeroData heroData)
    {
        return heroData.TeleportData.Graphics.icon;
    }
}
