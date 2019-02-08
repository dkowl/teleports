using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGraphicsID : MappedListID
{
    public ItemGraphicsID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Graphics.ItemGraphics.AllNames;
    }
}
