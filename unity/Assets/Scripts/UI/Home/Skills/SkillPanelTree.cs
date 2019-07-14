using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelTree : MonoBehaviour {

    public SkillTree skillTree_;
	
	void Awake () {

        int levels = skillTree_.levels.Count;

        for (int i = 0; i < levels; i++)
        {
            GameObject skillPanelLevel = Instantiate(Resources.Load("Prefabs/UI/Skills/SkillPanelLevel"), gameObject.transform) as GameObject;
            skillPanelLevel.GetComponent<SkillPanelLevel>().initialize(skillTree_.levels[i]);
        }
	}
}
