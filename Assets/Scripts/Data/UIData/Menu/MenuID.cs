using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuID : MappedListID
{

    public MenuID(string name) : base(name)
    {

    }

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.UI.Menus.AllNames;
    }
}
