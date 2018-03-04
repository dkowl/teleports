using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillData
{
    public int MaxCombo => MainData.Game.Combos.GetValue(ComboId).Data.DataIds.Count - 1;
}

