using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerGraphics {

	public static Sprite GetPlayerIcon(IPlayerData playerData)
    {
        return MainData.CurrentGameData.GetRace(playerData.RaceName).Graphics.Icon;
    }

    public static Sprite GetTeleportIcon(IPlayerData playerData)
    {
        return playerData.CurrentTeleportData.Graphics.icon;
    }
}
