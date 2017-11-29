using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class UnitAttributeData{

    private struct Data
    {
        public string name;

        public Data(string name)
        {
            this.name = name;
        }
    }

    private static Data[] data = new Data[Utils.EnumCount(typeof(UnitAttribute.Type))];

    static UnitAttributeData()
    {
        data[(int)UnitAttribute.Type.Size] = new Data("Size");
        data[(int)UnitAttribute.Type.Armor] = new Data("Armor");
        data[(int)UnitAttribute.Type.ArmorIgnore] = new Data("Piercing");
        data[(int)UnitAttribute.Type.Damage] = new Data("Damage");
        data[(int)UnitAttribute.Type.Height] = new Data("Height");
        data[(int)UnitAttribute.Type.Hp] = new Data("Health Points");
        data[(int)UnitAttribute.Type.MoveSpeed] = new Data("Movement Speed");
        data[(int)UnitAttribute.Type.Reach] = new Data("Reach");
        data[(int)UnitAttribute.Type.Regen] = new Data("Regeneration");
        data[(int)UnitAttribute.Type.RotationSpeed] = new Data("Rotation Speed");
        data[(int)UnitAttribute.Type.ViewRange] = new Data("ViewRange");
    }

    public static string GetName(UnitAttribute.Type type)
    {
        return data[(int)type].name;
    }
}
