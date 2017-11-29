using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class AttributeModifier : Perk {

	[System.Serializable]
    public class AttributeModificationSettings
    {
        [FormerlySerializedAs("type_")]
        public UnitAttribute.Type type;
        [FormerlySerializedAs("bonus_")]
        public float bonus = 0;
        [FormerlySerializedAs("multiplier_")]
        public float multiplier = 1;
    }

    public List<AttributeModificationSettings> attributeModificationSettings_;

    public override void Apply(Unit target)
    {
        base.Apply(target);

        foreach(AttributeModificationSettings ams in attributeModificationSettings_)
        {
            target.UnitData.GetAttribute(ams.type).Modify(ams.bonus, ams.multiplier);
        }
    }

    public override void unapply(Unit target)
    {
        base.Apply(target);

        foreach (AttributeModificationSettings ams in attributeModificationSettings_)
        {
            target.UnitData.GetAttribute(ams.type).Modify(-ams.bonus, 1/ams.multiplier);
        }
    }
}
