using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyID : MappedListID {

    protected override IList<string> DropdownValues()
    {
        return Main.StaticData.Game.Enemies.AllNames;
    }
}
