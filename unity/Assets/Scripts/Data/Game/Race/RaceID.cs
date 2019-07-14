using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceID : MappedListID
{
    public RaceID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Game.Races.AllNames;
    }
}
