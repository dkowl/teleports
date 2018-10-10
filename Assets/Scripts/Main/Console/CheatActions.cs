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
        if (Main.StaticData.Game.Items.ContainsName(name))
        {
            HeroData heroData = CurrentHeroData;
            if (heroData == null) return;

            heroData.UnitData?.Inventory?.Add(Main.StaticData.Game.Items.GetValue(name).GenerateItem());
            CheatConsole.Instance.Output(name + " added to inventory");
        }
        else
        {
            CheatConsole.Instance.Output("item " + name + " not found");
        }
    }

    public static void SetAttribute(string[] args)
    {
        if (args.Length < 3)
        {
            CheatConsole.Instance.Output("Not enough arguments");
        }

        UnitAttributesData.AttributeType type;
        if (!System.Enum.TryParse(args[1], out type))
        {
            Print($"attribute '{args[1]}' not found");
            return;
        }

        float value;
        if (!float.TryParse(args[2], out value))
        {
            Print("Inavlid value");
            return;
        }

        SetAttribute(type, value);
    }

    private static void SetAttribute(UnitAttributesData.AttributeType type, float value)
    {

        HeroData heroData = CurrentHeroData;
        Print($"{type} set from {heroData?.UnitData.Attributes.GetAttribute(type).Value} to {value}");
        heroData?.UnitData.Attributes.GetAttribute(type).Reset(value);
    }

    private static HeroData CurrentHeroData
    {
        get
        {
            HeroData result = Main.GameState.CurrentHeroData;
            if (result == null)
            {
                CheatConsole.Instance.Output("current player is null");
            }

            return result;
        }
    }

    private static void Print(string text)
    {
        CheatConsole.Instance.Output(text);
    }
}
