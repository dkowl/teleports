using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarValueInterpreter_HP : ProgressBarValueInterpreter
{
    private Unit unit;

    public ProgressBarValueInterpreter_HP(Unit unit)
    {
        this.unit = unit;
    }

    public override string NameTextString()
    {
        return unit.name;
    }
}
