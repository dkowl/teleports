using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheatActions  {

    public static void AddItem(string[] args)
    {
        AddItem(args[1]);
    }

    private static void AddItem(string name)
    {
        if (MainData.Game.Items.ContainsName(name))
        {
            PlayerData playerData = CurrentPlayerData;
            if (playerData == null) return;

            playerData.UnitData?.Inventory?.Add(MainData.Game.Items.GetValue(name).GenerateItem());
            CheatConsole.Instance.Output(name + " added to inventory");
        }
        else
        {
            CheatConsole.Instance.Output("item " + name + " not found");
        }
    }

    private static void SetAbility(UnitAbilities.Type type, int value)
    {

        PlayerData playerData = CurrentPlayerData;
        if (playerData == null) return;

        playerData.UnitData.Abilities.GetAbility(type);
    }

    private static PlayerData CurrentPlayerData
    {
        get
        {
            PlayerData result = MainData.CurrentPlayerData;
            if (result == null)
            {
                CheatConsole.Instance.Output("current player is null");
            }

            return result;
        }
    }
}
