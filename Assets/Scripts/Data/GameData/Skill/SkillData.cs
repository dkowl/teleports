using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillData
{
    public int MaxCombo => MainData.Game.Combos.GetValue(ComboId).Data.DataIds.Count - 1;
    public ComboData ComboData => ComboId != null ? MainData.Game.Combos.GetValue(ComboId).Data : null;


    public float AttacksPerSecond
    {
        get
        {
            switch (SkillType)
            {
                case SkillType.Attack:
                    return 1 / TotalCastTime;
                case SkillType.Combo:
                    int attacks = 0;
                    float time = 0;
                    foreach (var attack in ComboData.SkillDatas)
                    {
                        attacks++;
                        time += attack.TotalCastTime;
                    }
                    return attacks / time;
                default:
                    return 0;
            }
        }
    }
}

