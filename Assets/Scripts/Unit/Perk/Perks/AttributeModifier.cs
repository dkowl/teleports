using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModifier : Perk {

	[System.Serializable]
    public class AttributeModificationSettings
    {
        public Unit.AttributeType type_;
        public float bonus_ = 0, multiplier_ = 1;
    }

    public List<AttributeModificationSettings> attributeModificationSettings_;

    public override void apply(Unit target)
    {
        base.apply(target);

        foreach(AttributeModificationSettings ams in attributeModificationSettings_)
        {
            target.attributes_[(int)ams.type_].modify(ams.bonus_, ams.multiplier_);
        }
    }

    public override void unapply(Unit target)
    {
        base.apply(target);

        foreach (AttributeModificationSettings ams in attributeModificationSettings_)
        {
            target.attributes_[(int)ams.type_].modify(-ams.bonus_, 1/ams.multiplier_);
        }
    }
}
