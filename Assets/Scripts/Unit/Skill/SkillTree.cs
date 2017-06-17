using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skillTree", menuName = "Custom/Skill/Tree", order = 0)]
public class SkillTree : ScriptableObject {

    public SkillBranch rootBranch_;
    public List<SkillBranch> branches_;
}
