using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skillDatabase", menuName = "Custom/Skill/Database", order = 0)]
public class SkillDatabase : ScriptableObject {

    public List<SkillTree> skillTrees_;
}
