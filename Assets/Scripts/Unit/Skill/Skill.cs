using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Skill : MonoBehaviour {

    public class TargetInfo
    {
        private Unit targetUnit;
         Vector3 position;

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
                else return position;
            }
        }

    }

    public enum SkillType
    {
        Single,
        Area
    };
    
    [FormerlySerializedAs("name_")]
    [SerializeField]
    new private string name;

    [FormerlySerializedAs("type_")]
    [SerializeField]
    SkillType type;

    [FormerlySerializedAs("reach_")]
    [SerializeField]
    public Attribute reach;

    [FormerlySerializedAs("castTime_")]
    [SerializeField]
    public Attribute castTime;

    [FormerlySerializedAs("cooldown_")]
    [SerializeField]
    public Attribute cooldown;

    [FormerlySerializedAs("currentCooldown_")]
    [SerializeField]
    float currentCooldown;

    #region properties
    public SkillType Type
    {
        get { return type; }
    }
    public float Reach
    {
        get { return reach.Value(); }
    }
    public float CastTime
    {
        get { return castTime.Value(); }
    }
    public float Cooldown
    {
        get { return cooldown.Value(); }
    }
    public float CurrentCooldown
    {
        get { return currentCooldown; }
    }
    #endregion

    public SkillGraphics graphics;
    
    virtual public void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0) currentCooldown = 0;
    }

    public void Cast(Unit caster, TargetInfo target)
    {
        currentCooldown = cooldown.Value();

        foreach(Perk perk in caster.perks)
        {
            perk.onCast(caster, this, target);
        }

        InternalCast(caster, target);
    }

    abstract public void InternalCast(Unit caster, TargetInfo target);
}
