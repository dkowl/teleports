using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class ProgressBarValueInterpreter_XP : ProgressBarValueInterpreter {

    public override string NameTextString()
    {
        return "Level " + CurrentLevel.ToString();
    }

    public override string ValueTextString()
    {
        return CurrentLevel == Levels.xp.MaxLevel ? "Max" : base.ValueTextString();
    }

    private int CurrentLevel => Levels.xp.Level((int) currentValue + (int) minValue);
}
