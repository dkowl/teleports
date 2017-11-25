using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Skill : MonoBehaviour, IUniqueName {

    public class TargetInfo
    {
        Unit caster;
        TargetType targetType;
        Unit targetUnit;
        Vector3 targetPosition;        

        public TargetInfo(Unit caster, Unit targetUnit) 
            : this(caster, TargetType.Unit, targetUnit, Vector3.zero)
        { }

        public TargetInfo(Unit caster, Vector3 targetPosition)
            : this(caster, TargetType.Position, null, targetPosition)
        { }

        TargetInfo(Unit caster, TargetType targetType, Unit targetUnit, Vector3 targetPosition)
        {
            this.caster = caster;
            this.targetType = targetType;
            this.targetUnit = targetUnit;
            this.targetPosition = targetPosition;
        }

        public Unit Caster
        {
            get { return caster; }
        }

        public TargetType TargetType
        {
            get { return targetType; }
        }

        public Unit TargetUnit
        {
            get { return targetUnit; }
            set { targetUnit = value; }
        }

        public Vector3 Position
        {
            get
            {
                if (targetUnit != null)
                {
                    return TargetUnit.transform.position;
                }
                else return targetPosition;
            }
        }
    }

    public enum TargetType
    {
        Unit,
        Position
    };

    public enum AttributeType
    {
        Reach,
        CastTime,
        Cooldown
    }
    
    [FormerlySerializedAs("name_")]
    [SerializeField] new private string name;
    [FormerlySerializedAs("type_")]
    [SerializeField] private TargetType type;
    [FormerlySerializedAs("reach_")]
    [SerializeField] private Attribute reach;
    [FormerlySerializedAs("castTime_")]
    [SerializeField] private Attribute castTime;
    [FormerlySerializedAs("cooldown_")]
    [SerializeField] private Attribute cooldown;
    [SerializeField] private SkillGraphics graphics;

    float currentCooldown;

    virtual public void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0) currentCooldown = 0;
    }

    public void Cast(Unit caster, TargetInfo target)
    {
        currentCooldown = cooldown.Value;

        foreach(Perk perk in caster.perks)
        {
            perk.onCast(caster, this, target);
        }

        InternalCast(caster, target);
    }

    abstract public void InternalCast(Unit caster, TargetInfo target);

    public bool CanReachCastTarget(TargetInfo targetInfo)
    {
        Unit caster = targetInfo.Caster;
        if (caster != null)
        {
            float totalReach = caster.Reach + caster.Size + Reach;
            if (targetInfo.TargetUnit != null) totalReach += targetInfo.TargetUnit.Size;

            return
                (targetInfo.Position - caster.transform.position).magnitude
                <=
                totalReach;
        }
        else
        {
            return false;
        }
    }

    public void ModifyAttribute(AttributeType type, float bonus, float multiplier)
    {
        GetAttribute(type).Modify(bonus, multiplier);
    }

    Attribute GetAttribute(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.CastTime:
                return castTime;
            case AttributeType.Cooldown:
                return cooldown;
            case AttributeType.Reach:
                return reach;
            default:
                return null;
        }
    }

    public string UniqueName
    {
        get { return name; }
    }

    public TargetType Type
    {
        get { return type; }
    }

    public float Reach
    {
        get { return reach.Value; }
    }

    public float CastTime
    {
        get { return castTime.Value; }
    }

    public float Cooldown
    {
        get { return cooldown.Value; }
    }

    public float CurrentCooldown
    {
        get { return currentCooldown; }
    }

    public SkillGraphics Graphics
    {
        get { return graphics; }
    }
}
