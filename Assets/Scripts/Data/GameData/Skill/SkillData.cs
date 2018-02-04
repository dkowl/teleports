using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

[System.Serializable]
public class SkillData : IUniqueName {

    [SerializeField] private string uniqueName;
    [SerializeField] private Skill.TargetType targetType;
    [SerializeField] private Attribute reach;
    [SerializeField] private Attribute reachAngle = new Attribute(30);
    [SerializeField] private Attribute cooldown;
    [SerializeField] private Attribute castTime;
    [SerializeField] private Attribute totalCastTime;
    [SerializeField] private Attribute earlyBreakTime;
    [SerializeField] private int maxCombo;
    [SerializeField] private SkillGraphics graphics;

    public SkillData(SkillData other)
    {
        uniqueName = other.uniqueName;
        targetType = other.targetType;
        //todo
    }

    public Attribute GetAttribute(AttributeType type)
    {
        switch (type)
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

    private List<Attribute> GetAllAttributes()
    {
        var result = new List<Attribute>();
        //todo
        return result;
    }

    public string UniqueName { get { return uniqueName; } }
    public Skill.TargetType TargetType { get { return targetType; } }
    public float Reach { get { return reach.Value; } }
    public float ReachAngle { get { return reachAngle.Value; } }
    public float Cooldown { get { return cooldown.Value; } }
    public float CastTime { get { return castTime.Value; } }
    public float TotalCastTime { get { return totalCastTime.Value; } }
    public float EarlyBreakTime { get { return earlyBreakTime.Value; } }
    public int MaxCombo { get { return maxCombo; } }
    public SkillGraphics Graphics { get { return graphics; } }

    public enum AttributeType
    {
        Reach,
        ReachAngle,
        Cooldown,
        CastTime,
        TotalCastTime,
        EarlyBreakTime
    }
}
