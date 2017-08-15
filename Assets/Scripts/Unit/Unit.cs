using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public IUnitData unitData;

    string unitName;
    public int level;

    public enum AttributeType
    {
        Size,
        Hp,
        Armor,
        Regen,
        Damage,
        ArmorIgnore,
        Reach,
        MoveSpeed,
        ViewRange,
        Count
    }

    public Attribute[] attributes = new Attribute[(int)AttributeType.Count];

    #region attribute properties
    public float Size
    {
        get { return attributes[(int)AttributeType.Size].Value()/2f; }
    }

    public float Hp
    {
        get { return attributes[(int)AttributeType.Hp].Value(); }
    }

    public float Armor
    {
        get { return attributes[(int)AttributeType.Armor].Value(); }
    }

    public float Regen
    {
        get { return attributes[(int)AttributeType.Regen].Value(); }
    }

    public float Damage
    {
        get { return attributes[(int)AttributeType.Damage].Value(); }
    }

    public float ArmorIgnore
    {
        get { return attributes[(int)AttributeType.ArmorIgnore].Value(); }
    }

    public float Reach
    {
        get { return attributes[(int)AttributeType.Reach].Value(); }
    }

    public float MoveSpeed
    {
        get {
            return attributes[(int)AttributeType.MoveSpeed].Value();
        }
    }

    public float ViewRange
    {
        get { return attributes[(int)AttributeType.ViewRange].Value(); }
    }
    #endregion

    #region other properties

    public bool isMoving
    {
        get { return isMoving_; }
    }

    #endregion

    float rotationSpeed_;
    public float height;

    //hp
    float damageReceived_;
    bool isDead_;

    //pathfinding
    Vector3 moveDest_;
    bool isMoving_;

    Quaternion rotationTarget_;
    bool isRotating_;

    //special states
    bool isStunned_;
    float stunTime_;

    //skill casting
    Skill.TargetInfo castTarget_;
    Skill activeSkill_;
    float currentCastTime_;
    bool isCasting_;
    public List<Skill> skills_;

    //perks
    public List<Perk> perks_;

    //kill rewarding
    Unit lastAttacker_;

    //graphics
    UnitGraphics graphics_;
    public UnitGraphics Graphics
    {
        get { return graphics_; }
    }

    //controller
    public UnitController activeController_;

    //events
    //cast event
    public event EventHandler<CastEventArgs> castEvent;

    void Awake () {
        damageReceived_ = 0;
        rotationSpeed_ = 2;
        isMoving_ = false;
        isRotating_ = false;
        isCasting_ = false;
        isDead_ = false;
        isStunned_ = false;

        loadFromUnitData();

        graphics_ = gameObject.AddComponent<UnitGraphics>();
	}

    void Start()
    {
        applyPerks();
    }
	
	// Update is called once per frame
	void Update () {

        float dTime = Time.deltaTime;

        if (alive() && !isStunned_)
        {
            //Movement
            if (canMove())
            {
                Vector3 offset = moveDest_ - transform.position;

                if (MoveSpeed * dTime < offset.magnitude)
                {
                    offset *= MoveSpeed * dTime / offset.magnitude;
                }
                else
                {
                    isMoving_ = false;
                }

                transform.position += offset;

            }
            //Rotation
            if (isRotating_)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget_, dTime * rotationSpeed_ * 360);
                if (transform.rotation == rotationTarget_)
                {
                    isRotating_ = false;
                }
            }
            //Skill Casting
            if (isCasting_ && canReachCastTarget())
            {
                currentCastTime_ += dTime;
                if (currentCastTime_ >= activeSkill_.CastTime)
                {
                    activeSkill_.cast(this, castTarget_);
                    isCasting_ = false;
                    if(castEvent != null) castEvent(this, new CastEventArgs(activeSkill_));
                }
            }
            else
            {
                resetCast();
            }
        }
        //Stun
        else if (isStunned_)
        {
            stunTime_ -= dTime;
            if(stunTime_<= 0)
            {
                resetStun();
            }
        }
   
    }

    public void cast(Skill skill, Skill.TargetInfo target)
    {
        if (isCasting_ && activeSkill_ == skill && castTarget_ == target) return;
        isMoving_ = false;
        if(skill.CurrentCooldown == 0)
        {
            isCasting_ = true;
            activeSkill_ = skill;
            castTarget_ = target;
            currentCastTime_ = 0;
        }
    }

    public void moveTo(Vector3 moveDest)
    {
        if (moveDest != transform.position)
        {
            moveDest_ = moveDest;
            rotationTarget_ = Quaternion.LookRotation(moveDest_ - transform.position);
            isMoving_ = true;
            isRotating_ = true;
        }
    }

    public void addPerk(Perk perk)
    {
        perk.apply(this);
        perks_.Add(perk);
    }

    public bool alive()
    {
        return !isDead_;
    }

    void applyPerks()
    {
        foreach(Perk perk in perks_)
        {
            perk.apply(this);
        }
    }

    bool canMove()
    {
        return !isCasting_ && isMoving_ && !isDead_;
    }

    public bool canReachCastTarget(Skill skill, Skill.TargetInfo target)
    {
        float totalReach = Reach + Size + skill.Reach;
        if (target.unit != null) totalReach += target.unit.Size;

        return
            (target.position - transform.position).magnitude
            <=
            totalReach;
    }

    bool canReachCastTarget()
    {
        return canReachCastTarget(activeSkill_, castTarget_);
    }

    void loadFromUnitData()
    {
        if(unitData != null)
        {
            unitName = unitData.Name;
            level = unitData.Level;
            attributes[(int)AttributeType.Size] = new Attribute(unitData.Size);
            attributes[(int)AttributeType.Hp] = new Attribute(unitData.Hp);
            attributes[(int)AttributeType.Armor] = new Attribute(unitData.Armor);
            attributes[(int)AttributeType.Regen] = new Attribute(unitData.Regen);
            attributes[(int)AttributeType.Damage] = new Attribute(unitData.Damage);
            attributes[(int)AttributeType.ArmorIgnore] = new Attribute(unitData.ArmorIgnore);
            attributes[(int)AttributeType.Reach] = new Attribute(unitData.Reach);
            attributes[(int)AttributeType.MoveSpeed] = new Attribute(unitData.MoveSpeed);
            attributes[(int)AttributeType.ViewRange] = new Attribute(unitData.ViewRange);
            height = unitData.Height;
        }
    }

    public void receiveDamage(float damage, Unit attacker)
    {
        float actualDamage = Mathf.Max(damage - Mathf.Max(Armor - attacker.ArmorIgnore, 0), 0);
        damageReceived_ += actualDamage;
        if(actualDamage > 0) lastAttacker_ = attacker;
        graphics_.showDamage(actualDamage);
        if(damageReceived_ >= Hp)
        {
            die();
        }
    }

    public void removePerk(Perk perk)
    {
        perks_.Remove(perk);
        perk.unapply(this);
    }

    public void resetCast()
    {
        isCasting_ = false;
        activeSkill_ = null;
        currentCastTime_ = 0;
        castTarget_ = null;
    }

    public void resetStun()
    {
        isStunned_ = false;
        stunTime_ = 0;
    }

    public void stun(float time)
    {
        isStunned_ = true;
        stunTime_ += time;
        graphics_.showMessage("Stunned!");
    }

    public void die()
    {
        if (isDead_) return;
        isDead_ = true;
        if(lastAttacker_ != null)
        {
            Xp xp = lastAttacker_.gameObject.GetComponent<Xp>();
            if (xp != null)
            {
                xp.receiveXp((1000 * level));
            }
        }
    }

    public float healthPercentage()
    {
        return 1f - (damageReceived_ / Hp);
    }
}
