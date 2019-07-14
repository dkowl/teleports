using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargeter_Point : SkillTargeter {

    public override List<CastTarget> GetTargets(Skill skill, Skill.TargetInfo targetInfo)
    {
        if (targetInfo.TargetUnit == null) return null;
        var result = new List<CastTarget>();
        if (skill.CanReachTarget(targetInfo))
        {
            result.Add(new CastTarget(targetInfo.TargetUnit, CastTarget.TypeFlag.Primary));
        }
        else
        {
            targetInfo.TargetUnit.Graphics.showMessage("Miss!");
        }
        return result;
    }
}
