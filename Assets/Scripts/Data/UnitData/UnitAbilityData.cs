using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public static class UnitAbilityData {

    private struct Data
    {
        public string name;
        public Color color;

        public Data(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }

    private static Data[] data = new Data[Utils.EnumCount(typeof(UnitAbilities.Type))];

    static UnitAbilityData()
    {
        data[(int)UnitAbilities.Type.STR] = new Data("Strength", Color.red);
        data[(int)UnitAbilities.Type.DEX] = new Data("Dexterity", Color.green);
        data[(int)UnitAbilities.Type.INT] = new Data("Intelligence", Color.blue);
    }

    public static string GetName(UnitAbilities.Type type)
    {
        return data[(int)type].name;
    }

    public static Color GetColor(UnitAbilities.Type type)
    {
        return data[(int)type].color;
    }
}
