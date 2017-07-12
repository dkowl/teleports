using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skillTree", menuName = "Custom/Skill/Tree", order = 0)]
public class SkillTree : ScriptableObject {

    [System.Serializable]
    public class Level
    {
        public List<SkillBranch> branches_;
    }

    public List<Level> levels_;

    public Color color_;
}
