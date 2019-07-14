using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SkillTargeter { 

    public abstract List<CastTarget> GetTargets(Skill skill, Skill.TargetInfo targetInfo);
    
}
