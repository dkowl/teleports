using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargeter_Cone : SkillTargeter {

    [SerializeField] float coneAngle;
    [SerializeField] Vector3 originPoint;
    [SerializeField] Vector3 coneVector;


	public override List<CastTarget> GetTargets(Skill.TargetInfo targetInfo)
    {
        var result = new List<CastTarget>();

        return result;
    }
}
