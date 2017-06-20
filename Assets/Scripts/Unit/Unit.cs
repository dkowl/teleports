using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public UnitData unitData_;

    string name_;
    public int level_;

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

    public Attribute[] attributes_ = new Attribute[(int)AttributeType.Count];

    #region attribute properties
    public float Size
    {
        get { return attributes_[(int)AttributeType.Size].value()/2f; }
    }

    public float Hp
    {
        get { return attributes_[(int)AttributeType.Hp].value(); }
    }

    public float Armor
    {
        get { return attributes_[(int)AttributeType.Armor].value(); }
    }

    public float Regen
    {
        get { return attributes_[(int)AttributeType.Regen].value(); }
    }

    public float Damage
    {
        get { return attributes_[(int)AttributeType.Damage].value(); }
    }

    public float ArmorIgnore
    {
        get { return attributes_[(int)AttributeType.ArmorIgnore].value(); }
    }

    public float Reach
    {
        get { return attributes_[(int)AttributeType.Reach].value(); }
    }

    public float MoveSpeed
    {
        get { return attributes_[(int)AttributeType.MoveSpeed].value(); }
    }

    public float ViewRange
    {
        get { return attributes_[(int)AttributeType.ViewRange].value(); }
    }
    #endregion

    float rotationSpeed_;
    public float height_;

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
                }
            }
            else
            {
                resetCast();
            }
        }
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
        if(unitData_ != null)
        {
            name_ = unitData_.name_;
            level_ = unitData_.level_;
            attributes_[(int)AttributeType.Size].raw_ = unitData_.size_;
            attributes_[(int)AttributeType.Hp].raw_ = unitData_.hp_;
            attributes_[(int)AttributeType.Armor].raw_ = unitData_.armor_;
            attributes_[(int)AttributeType.Regen].raw_ = unitData_.regen_;
            attributes_[(int)AttributeType.Damage].raw_ = unitData_.damage_;
            attributes_[(int)AttributeType.ArmorIgnore].raw_ = unitData_.armorIgnore_;
            attributes_[(int)AttributeType.Reach].raw_ = unitData_.reach_;
            attributes_[(int)AttributeType.MoveSpeed].raw_ = unitData_.moveSpeed_;
            attributes_[(int)AttributeType.ViewRange].raw_ = unitData_.viewRange_;
            height_ = unitData_.height_;
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
                xp.receiveXp((1000 * level_));
            }
        }
    }

    public float healthPercentage()
    {
        return 1f - (damageReceived_ / Hp);
    }
}
