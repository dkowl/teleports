using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboID : MappedListID
{
    public ComboID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Game.Combos.AllNames;
    }
}
