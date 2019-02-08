using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldID : MappedListID
{

    public WorldID(string name) : base(name)
    {

    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Game.Worlds.AllNames;
    }
}
