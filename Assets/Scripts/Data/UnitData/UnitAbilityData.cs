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

    private static Data[] data = new Data[Utils.EnumCount(typeof(UnitAbility.Type))];

    static UnitAbilityData()
    {
        data[(int)UnitAbility.Type.STR] = new Data("Strength", Color.red);
        data[(int)UnitAbility.Type.DEX] = new Data("Dexterity", Color.green);
        data[(int)UnitAbility.Type.INT] = new Data("Intelligence", Color.blue);
    }

    public static string GetName(UnitAbility.Type type)
    {
        return data[(int)type].name;
    }

    public static Color GetColor(UnitAbility.Type type)
    {
        return data[(int)type].color;
    }
}
