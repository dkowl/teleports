using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class AttributeModifier : Perk {

	[System.Serializable]
    public class AttributeModificationSettings
    {
        [FormerlySerializedAs("type_")] public UnitAttributes.Type type;
        [FormerlySerializedAs("bonus_")] public float bonus = 0;
        [FormerlySerializedAs("multiplier_")] public float multiplier = 1;
    }

    public List<AttributeModificationSettings> attributeModificationSettings;

    protected override void ApplyInternal(Unit target)
    {
        foreach(AttributeModificationSettings ams in attributeModificationSettings)
        {
            target.UnitData.GetAttribute(ams.type).Modify(ams.bonus, ams.multiplier);
        }
    }

    protected override void UnapplyInternal(Unit target)
    {
        foreach (AttributeModificationSettings ams in attributeModificationSettings)
        {
            target.UnitData.GetAttribute(ams.type).Modify(-ams.bonus, 1/ams.multiplier);
        }
    }
}
