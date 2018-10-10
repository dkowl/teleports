using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : Skill
{
    private ComboData comboData;
    private List<Skill> skills;

    protected override void OnInitialize()
    {
        comboData = new ComboData(Main.StaticData.Game.Combos.GetValue(Data.ComboId).Data);
        skills = new List<Skill>();
        foreach (SkillData skillData in comboData.SkillDatas)
        {
            skills.Add(SkillSpawner.SpawnSkill(gameObject, skillData, unit));
        }
        data = comboData.SkillDatas[comboData.DataIds[0]];
    }

    protected override void CastInternal(Unit caster, List<CastTarget> targets)
    {
        Debug.Log("Casting my sexy combo");
        CurrentSkill.Cast(caster, targets);
    }

    public override void RegisterCombo(int counter)
    {
        base.RegisterCombo(counter);
        Debug.LogFormat("Registering my sexy combo, ComboCounter is: {0}", ComboCounter);
        data = comboData.SkillDatas[comboData.DataIds[ComboCounter]];
    }

    public override CanReachTargetResult CanReachTarget(TargetInfo targetInfo)
    {
        //Debug.Log("Can I reach my target???");
        return CurrentSkill.CanReachTarget(targetInfo);
    }

    public override float GetReach(Unit caster)
    {
        //Debug.Log("Calling Combo.GetReach()");
        return CurrentSkill.GetReach(caster);
    }

    public override float GetSpeedModifier(Unit caster)
    {
        return CurrentSkill.GetSpeedModifier(caster);
    }

    protected Skill CurrentSkill => skills[comboData.DataIds[ComboCounter]];
    public override int MaxCombo => comboData.DataIds.Count;
}
