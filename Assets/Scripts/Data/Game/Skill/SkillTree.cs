using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "skillTree", menuName = "Custom/Skill/Tree", order = 0)]
public class SkillTree : ScriptableObject {

    [System.Serializable]
    public class Level
    {
        [FormerlySerializedAs("branches_")]
        public List<SkillBranch> branches;
    }

    [FormerlySerializedAs("levels_")]
    public List<Level> levels;

    [FormerlySerializedAs("color_")]
    public Color color;
}
