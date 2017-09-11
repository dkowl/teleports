using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "skillDatabase", menuName = "Custom/Skill/Database", order = 0)]
public class SkillDatabase : ScriptableObject {

    [FormerlySerializedAs("skillTrees_")]
    [SerializeField]
    private List<SkillTree> skillTrees;

    [SerializeField]
    private List<Skill> skills;

    public Skill GetSkill(SkillID id)
    {
        return null;
    }
}
