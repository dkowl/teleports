﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

[System.Serializable]
public class ItemGraphicsID : MappedListID
{
    public ItemGraphicsID(string name) : base(name)
    {
    }

    protected override IList<string> DropdownValues()
    {
        return MainData.Game.GraphicsData.ItemGraphics.AllNames;
    }
}
