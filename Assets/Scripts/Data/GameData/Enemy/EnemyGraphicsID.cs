using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicsID : MappedListID {

    protected override IList<string> DropdownValues()
    {
        return MainData.CurrentGameData.GraphicsData.EnemyGraphicsNames;
    }
}
