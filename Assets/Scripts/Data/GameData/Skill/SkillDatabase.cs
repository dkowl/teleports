using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "skillDatabase", menuName = "Custom/Skill/Database", order = 0)]
public class SkillDatabase : ScriptableObject {

    [FormerlySerializedAs("skillTrees_")]
    [SerializeField]
    private List<SkillTree> skillTrees;

    public Skill GetSkill(SkillID id)
    {
        if(id.elementID != 0)
        {
            Debug.Log("Invalid element Id (must be 0 for skills, >0 for perks)");
        }

        if(id.treeID >= skillTrees.Count)
        {
            Debug.Log("Invalid tree ID");
            return null;
        }
        SkillTree tree = skillTrees[id.treeID];
        int branchCount = -1;
        foreach(SkillTree.Level level in tree.levels)
        {
            foreach(SkillBranch branch in level.branches)
            {
                branchCount++;
                if(branchCount == id.branchID)
                {
                    return branch.rootSkill_;
                }
            }
        }
        Debug.Log("Invalid branch ID");
        return null;
    }
}
