using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillData
{

    public void PopulateFromSkill(Skill skill)
    {
        uniqueName = skill.UniqueName;
        targetType = skill.Type;
        reach = new Attribute(skill.Reach);
        reachAngle = new Attribute(skill.ReachAngle);
        cooldown = new Attribute(skill.Cooldown);
        castTime = new Attribute(skill.CastTime);
        totalCastTime = new Attribute(skill.TotalCastTime);
        earlyBreakTime = new Attribute(skill.EarlyBreakTime);
        maxCombo = skill.MaxCombo;
        graphics = skill.Graphics;
        var attack = skill as Attack;
        if (attack != null)
        {
            skillType = SkillType.Attack;
            attackData.PopulateFromAttack(attack);
        }
    }
}

