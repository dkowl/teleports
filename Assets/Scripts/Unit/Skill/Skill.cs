using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {

    public class TargetInfo
    {
        Unit unit_;
        public Unit unit
        {
            get { return unit_; }
            set { unit_ = value; }
        }
        public Vector3 position_;
        public Vector3 position
        {
            get
            {
                if (unit_ != null)
                {
                    return unit.transform.position;
                }
                else return position_;
            }
        }
    }

    public enum Type
    {
        Single,
        Area
    };

    Type type_;
    public Type type
    {
        get { return type_; }
    }

    public string name_;

    public Attribute
        reach_,
        castTime_,
        cooldown_;

    #region attribute properties
    public float Reach
    {
        get { return reach_.value(); }
    }
    public float CastTime
    {
        get { return castTime_.value(); }
    }
    public float Cooldown
    {
        get { return cooldown_.value(); }
    }
    #endregion

    public float currentCooldown_;
    public float CurrentCooldown
    {
        get { return currentCooldown_; }
    }
    
    void Update()
    {
        currentCooldown_ -= Time.deltaTime;
        if (currentCooldown_ < 0) currentCooldown_ = 0;
    }

    public void cast(Unit caster, TargetInfo target)
    {
        currentCooldown_ = cooldown_.value();

        internalCast(caster, target);
    }

    abstract public void internalCast(Unit caster, TargetInfo target);
}
