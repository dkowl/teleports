using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarUI : BaseProgressBarUI {

    protected override float CurrentValue()
    {
        return XpLevels.CurrentXp(MainData.CurrentPlayerData.Xp);
    }

    protected override float MaxValue()
    {
        return XpLevels.RequiredXp(MainData.CurrentPlayerData.Xp);
    }
}
