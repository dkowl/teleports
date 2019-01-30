using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class ProgressBarValueInterpreter_XP : ProgressBarValueInterpreter {

    public override BasicProgressBar.Values InterpretValues(float oldXp, float newXp)
    {
        return new BasicProgressBar.Values
        {
            current = Levels.xp.AboveCurrentLevel((int)oldXp),
            target = Levels.xp.AboveCurrentLevel((int)oldXp) + newXp - oldXp,
            delta = newXp - oldXp,
            min = 0,
            max = Levels.xp.RequiredFromCurrentLevelToNext((int)oldXp)
        };
    }

    public override string NameTextString()
    {
        return "Level " + CurrentLevel.ToString();
    }

    public override string ValueTextString()
    {
        return CurrentLevel == Levels.xp.MaxLevel ? "Max" : base.ValueTextString();
    }

    private int CurrentLevel => Levels.xp.LevelByRequiredFromCurrentLevelToNext((int)values.max);
}
