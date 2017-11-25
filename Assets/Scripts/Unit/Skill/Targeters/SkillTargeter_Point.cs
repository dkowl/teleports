using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargeter_Point : SkillTargeter {

    public override List<CastTarget> GetTargets(Skill.TargetInfo targetInfo)
    {
        var result = new List<CastTarget>();
        if(targetInfo.TargetUnit != null)
            result.Add(new CastTarget(targetInfo.TargetUnit, CastTarget.TypeFlag.Primary));
        return result;
    }
}
