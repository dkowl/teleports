using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillGraphicsID : MappedListID
{
    public SkillGraphicsID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return MainData.Game.GraphicsData.SkillGraphics.AllNames;
    }
}
