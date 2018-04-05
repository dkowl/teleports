using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceGraphicsID : MappedListID
{
    public RaceGraphicsID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return MainData.Game.GraphicsData.RaceGraphicsNames;
    }
}
