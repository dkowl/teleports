using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class PerkID : MappedListID {

    protected override IList<string> DropdownValues()
    {
        return MainData.CurrentGameData.PerkNames;
    }

}
