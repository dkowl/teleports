using System;
using System.Collections;
using System.Collections.Generic;

public class CastEventArgs : EventArgs {

	public CastEventArgs(Skill skill)
    {
        skill_ = skill;
    }

    private Skill skill_;

    public Skill Skill
    {
        get { return skill_; }
        set { skill_ = value; }
    }
}
