using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//class used as a skill identifier
//you can use skill's position in the skill tree or its name
[System.Serializable]
public class SkillID : MappedListID
{
    [HideIf("UsesString")]
    public byte
        treeID,
        branchID,
        elementID;

    public SkillID() : base(DataDefaults.skillName) { }

    public SkillID(string name) : base(name) { }

    public bool UsesString()
    {
        return Name.Length > 0;
    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Game.Skills.AllNames;
    }
}
