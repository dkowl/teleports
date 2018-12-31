using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class ProgressBarValueInterpreter_RP : ProgressBarValueInterpreter {

    public override string NameTextString()
    {
        return "Rank " + RomanNumbers.RomanNumber(Levels.rp.Level((int)currentValue));
    }

    public override string ValueTextString()
    {
        if (valueTextType == BasicProgressBar.ValueTextType.OneValue)
        {
            return currentValue.ToString("F0");
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
                return Levels.rp.Owned((int)currentValue).ToString();
            case 1:
                return (Levels.rp.Owned((int)currentValue) + Levels.rp.Required((int)currentValue)).ToString();
            default:
                return base.SecondaryTextString(id);
        }
    }
}
