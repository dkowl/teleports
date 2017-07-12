using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelBranch : MonoBehaviour {

    SkillBranch branch_;
    Image image_;

    public void initialize(SkillBranch branch)
    {
        if (branch == null) return;

        branch_ = branch;

        image_ = GetComponent<Image>();

        image_.sprite = branch_.rootSkill_.graphics_.uiIcon_;

    }
}
