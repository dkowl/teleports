using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelLevel : MonoBehaviour {

    SkillTree.Level level_;

    public void initialize(SkillTree.Level level)
    {
        level_ = level;

        for(int i = 0; i<level_.branches.Count; i++)
        {
            GameObject skillPanelBranch = Instantiate(Resources.Load("Prefabs/UI/Skills/SkillPanelBranch"), gameObject.transform) as GameObject;
            skillPanelBranch.GetComponent<SkillPanelBranch>().initialize(level_.branches[i]);
        }
    }
}
