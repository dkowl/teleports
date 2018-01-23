using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceGraphicsID : MappedListID {

    protected override IList<string> DropdownValues()
    {
        return MainData.CurrentGameData.GraphicsData.RaceGraphicsNames;
    }
}
