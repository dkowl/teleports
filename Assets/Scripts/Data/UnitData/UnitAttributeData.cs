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

    private static Data[] data = new Data[Utils.EnumCount(typeof(UnitAttributes.Type))];

    static UnitAttributeData()
    {
        data[(int)UnitAttributes.Type.Size] = new Data("Size");
        data[(int)UnitAttributes.Type.Armor] = new Data("Armor");
        data[(int)UnitAttributes.Type.Height] = new Data("Height");
        data[(int)UnitAttributes.Type.Hp] = new Data("Health Points");
        data[(int)UnitAttributes.Type.MoveSpeed] = new Data("Movement Speed");
        data[(int)UnitAttributes.Type.Reach] = new Data("Reach");
        data[(int)UnitAttributes.Type.Regen] = new Data("Regeneration");
        data[(int)UnitAttributes.Type.RotationSpeed] = new Data("Rotation Speed");
        data[(int)UnitAttributes.Type.ViewRange] = new Data("View Range");
    }

    public static string GetName(UnitAttributes.Type type)
    {
        return data[(int)type].name;
    }
}
