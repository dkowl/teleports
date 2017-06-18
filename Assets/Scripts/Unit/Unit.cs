using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    string name_;
    public int level_;

    public Attribute
        size_,
        hp_,
        armor_,
        regen_,
        damage_,
        reach_,
        moveSpeed_,
        viewRange_;

    #region attribute properties
    public float Size
    {
        get { return size_.value()/2f; }
    }

    public float Hp
    {
        get { return hp_.value(); }
    }

    public float Armor
    {
        get { return armor_.value(); }
    }

    public float Regen
    {
        get { return regen_.value(); }
    }

    public float Damage
    {
        get { return damage_.value(); }
    }

    public float Reach
    {
        get { return reach_.value(); }
    }

    public float MoveSpeed
    {
        get { return moveSpeed_.value(); }
    }

    public float ViewRange
    {
        get { return viewRange_.value(); }
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

    void Awake () {
        damageReceived_ = 0;
        rotationSpeed_ = 2;
        isMoving_ = false;
        isRotating_ = false;
        isCasting_ = false;
        isDead_ = false;

        graphics_ = gameObject.AddComponent<UnitGraphics>();
	}

    void Start()
    {
        applyPerks();
    }
	
	// Update is called once per frame
	void Update () {

        float dTime = Time.deltaTime;

        if (alive())
        {
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
            if (isRotating_)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget_, dTime * rotationSpeed_ * 360);
                if (transform.rotation == rotationTarget_)
                {
                    isRotating_ = false;
                }
            }

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

    public void receiveDamage(float damage, Unit attacker)
    {
        damageReceived_ += damage;
        lastAttacker_ = attacker;
        graphics_.showDamage(damage);
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
