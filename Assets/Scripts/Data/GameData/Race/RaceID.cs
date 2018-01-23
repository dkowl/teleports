using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceID : MappedListID
{

    protected override IList<string> DropdownValues()
    {
        return MainData.CurrentGameData.RaceNames;
    }
}
