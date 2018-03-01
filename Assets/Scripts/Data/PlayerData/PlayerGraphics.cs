using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerGraphics {

	public static Sprite GetPlayerIcon(PlayerData playerData)
    {
        return MainData.Game.GetRace(playerData.RaceName).Graphics.Icon;
    }

    public static Sprite GetTeleportIcon(PlayerData playerData)
    {
        return playerData.TeleportData.Graphics.icon;
    }
}
