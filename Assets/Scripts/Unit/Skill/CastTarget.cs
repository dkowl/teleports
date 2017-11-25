using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CastTarget
{
    public enum TypeFlag
    {
        Primary = 0x1,
        Secondary = 0x2,
        Both = Primary | Secondary
    }

    Unit unit;
    TypeFlag type;

    public CastTarget(Unit unit, TypeFlag type)
    {
        this.unit = unit;
        this.type = type;
    }

    public Unit Unit
    {
        get { return unit; }
    }

    public TypeFlag Type
    {
        get { return type; }
    }
}
