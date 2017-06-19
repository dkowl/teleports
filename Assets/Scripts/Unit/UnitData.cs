using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unitData", menuName = "Custom/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    public string name_;
    public int level_;
    public float
        size_,
        hp_,
        armor_,
        regen_,
        damage_,
        armorIgnore_,
        reach_,
        moveSpeed_,
        viewRange_,
        height_;
}
