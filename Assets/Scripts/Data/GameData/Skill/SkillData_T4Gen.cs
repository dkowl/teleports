//DO NOT EDIT - Autogenerated file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public partial class SkillData : IDeepCopyable , IUniqueName {

	[SerializeField] private string uniqueName;
	[SerializeField] private Skill.TargetType targetType;
	[SerializeField] private Attribute reach;
	[SerializeField] private Attribute reachAngle;
	[SerializeField] private Attribute cooldown;
	[SerializeField] private Attribute castTime;
	[SerializeField] private Attribute totalCastTime;
	[SerializeField] private Attribute earlyBreakTime;
	[SerializeField] private SkillType skillType;
	[SerializeField] private AttackData attackData;
	[SerializeField] private ComboID comboId;
	[SerializeField] private bool blockRotation;
	[SerializeField] private SkillGraphics graphics;

	public SkillData() {
		blockRotation = true;
	}

	public SkillData(SkillData other){
		uniqueName = other.uniqueName;
		targetType = other.targetType;
		reach = new Attribute(other.reach);
		reachAngle = new Attribute(other.reachAngle);
		cooldown = new Attribute(other.cooldown);
		castTime = new Attribute(other.castTime);
		totalCastTime = new Attribute(other.totalCastTime);
		earlyBreakTime = new Attribute(other.earlyBreakTime);
		skillType = other.skillType;
		attackData = new AttackData(other.attackData);
		comboId = new ComboID(other.comboId);
		blockRotation = other.blockRotation;
		graphics = other.graphics;
	}

	public object DeepCopy(){
		return new SkillData(this);
	}

	public Attribute GetAttribute(AttributeType type)
	{
		switch(type)
		{
			case AttributeType.Reach:
				return reach;
			case AttributeType.ReachAngle:
				return reachAngle;
			case AttributeType.Cooldown:
				return cooldown;
			case AttributeType.CastTime:
				return castTime;
			case AttributeType.TotalCastTime:
				return totalCastTime;
			case AttributeType.EarlyBreakTime:
				return earlyBreakTime;
			default:
				return null;
		}
	}

    public void ModifyAttribute(AttributeType type, float bonus, float multiplier)
	{
		GetAttribute(type).Modify(bonus, multiplier);
	}

	public string UniqueName => uniqueName;
	public Skill.TargetType TargetType => targetType;
	public float Reach => reach.Value;
	public float ReachAngle => reachAngle.Value;
	public float Cooldown => cooldown.Value;
	public float CastTime => castTime.Value;
	public float TotalCastTime => totalCastTime.Value;
	public float EarlyBreakTime => earlyBreakTime.Value;
	public SkillType SkillType => skillType;
	public AttackData AttackData => attackData;
	public ComboID ComboId => comboId;
	public bool BlockRotation => blockRotation;
	public SkillGraphics Graphics => graphics;

	public enum AttributeType {
		Reach,
		ReachAngle,
		Cooldown,
		CastTime,
		TotalCastTime,
		EarlyBreakTime,
	}

}
	