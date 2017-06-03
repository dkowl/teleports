using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unitData", menuName = "Custom/UnitData", order = 1)]
public class UnitData : ScriptableObject
{

    public string name_;
    public int 
        hp_,
        damage;
    public float
        size_,
        attackRange_,
        attackTime_,
        attackCooldown_,
        moveSpeed_,
        viewRange_;
}
