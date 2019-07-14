using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class ProgressBarValueInterpreter_RP : ProgressBarValueInterpreter {

    public override BasicProgressBar.Values InterpretValues(float oldRp, float newRp)
    {
        return new BasicProgressBar.Values
        {
            current = oldRp,
            target = newRp,
            delta = newRp - oldRp,
            min = Levels.rp.RequiredTotalForCurrentLevel((int) oldRp),
            max = Levels.rp.RequiredTotalForNextLevel((int) oldRp)
        };
    }

    public override string NameTextString()
    {
        return "Rank " + RomanNumbers.RomanNumber(Levels.rp.Level((int)values.current));
    }

    public override string ValueTextString()
    {
        if (valueTextType == BasicProgressBar.ValueTextType.OneValue)
        {
            return values.current.ToString("F0");
        }
        else
        {
            return base.ValueTextString();
        }
    }

    public override string SecondaryTextString(int id)
    {
        switch (id)
        {
            case 0:
                return Levels.rp.RequiredTotalForCurrentLevel((int)values.current).ToString();
            case 1:
                return (
                    Levels.rp.RequiredTotalForCurrentLevel((int)values.current) + 
                    Levels.rp.RequiredFromCurrentLevelToNext((int)values.current)).ToString();
            default:
                return base.SecondaryTextString(id);
        }
    }
}
