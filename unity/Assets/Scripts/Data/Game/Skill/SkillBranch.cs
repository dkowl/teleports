using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skillBranch", menuName = "Custom/Skill/Branch", order = 2)]
public class SkillBranch : ScriptableObject {

    public Skill rootSkill_;

    [System.Serializable]
    public class PerkLevel
    {
        public List<Perk> perks_;
    }
    public List<PerkLevel> perkLevels_;
    public List<SkillBranch> branches_;


}
