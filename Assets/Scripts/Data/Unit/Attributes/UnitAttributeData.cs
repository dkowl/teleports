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

    private static Data[] data = new Data[Utils.EnumCount(typeof(UnitAttributesData.AttributeType))];

    static UnitAttributeData()
    {
        data[(int)UnitAttributesData.AttributeType.Strength] = new Data("Strength");
        data[(int)UnitAttributesData.AttributeType.Dexterity] = new Data("Dexterity");
        data[(int)UnitAttributesData.AttributeType.Intelligence] = new Data("Intelligence");
        data[(int)UnitAttributesData.AttributeType.Size] = new Data("Size");
        data[(int)UnitAttributesData.AttributeType.Armor] = new Data("Armor");
        data[(int)UnitAttributesData.AttributeType.Height] = new Data("Height");
        data[(int)UnitAttributesData.AttributeType.HealthPoints] = new Data("Health Points");
        data[(int)UnitAttributesData.AttributeType.MovementSpeed] = new Data("Movement Speed");
        data[(int)UnitAttributesData.AttributeType.Reach] = new Data("Reach");
        data[(int)UnitAttributesData.AttributeType.Regeneration] = new Data("Regeneration");
        data[(int)UnitAttributesData.AttributeType.RotationSpeed] = new Data("Rotation Speed");
        data[(int)UnitAttributesData.AttributeType.ViewRange] = new Data("View Range");
    }

    public static string GetName(UnitAttributesData.AttributeType type)
    {
        return data[(int)type].name;
    }
}
