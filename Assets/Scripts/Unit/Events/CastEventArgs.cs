using System;
using System.Collections;
using System.Collections.Generic;

public class CastEventArgs : EventArgs {

	public CastEventArgs(Skill skill)
    {
        this.skill = skill;
    }

    private Skill skill;

    public Skill Skill
    {
        get { return skill; }
        set { skill = value; }
    }
}
