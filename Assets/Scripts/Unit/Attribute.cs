using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public float raw;
    float bonus, modifier;

    public float value()
    {
        return (raw + bonus) * (modifier + 1);
    }
};
