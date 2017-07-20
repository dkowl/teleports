using System;
using System.Collections.Generic;


[System.Serializable]
public class PlayerState {

    //main attributes
    string name_;
    int 
        level_,
        xp_,
        rankPoints_;

    SkillPointManager skillPointManager_;

    //dictionary key lists
    List<string>
        skills_,
        perks_,
        items_;

    PlayerState()
    {
        skillPointManager_ = new SkillPointManager(this);
    }
	
}
