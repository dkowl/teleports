using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
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
        RotationSpeed,
        ViewRange,
        Count
    }

    public UnitDataEditor unitDataEditor;
    public UnitData unitData;

    private float rotationSpeed;

    //hp
    private float damageReceived;
    private bool isDead;

    //pathfinding
    private Vector3 moveDest;
    private bool isMoving;

    private Quaternion rotationTarget;
    private bool isRotating;

    //special states
    private bool isStunned;
    private float stunTime;

    //skill casting
    private Skill.TargetInfo castTarget;
    private Skill activeSkill;
    private float currentCastTime;
    private bool isCasting;
    public List<Skill> skills;

    //perks
    public List<Perk> perks;

    //kill rewarding
    private Unit lastAttacker;

    //graphics
    private UnitGraphics graphics;
    public UnitGraphics Graphics
    {
        get { return graphics; }
    }

    //controller
    private UnitController activeController;

    //events
    //cast event
    public event EventHandler<CastEventArgs> castEvent;

    #region attribute properties
    public float Size
    {
        get { return unitData.GetAttribute(AttributeType.Size).GetValue() / 2f; }
    }

    public float Hp
    {
        get { return unitData.GetAttribute(AttributeType.Hp).GetValue(); }
    }

    public float Armor
    {
        get { return unitData.GetAttribute(AttributeType.Armor).GetValue(); }
    }

    public float Regen
    {
        get { return unitData.GetAttribute(AttributeType.Regen).GetValue(); }
    }

    public float Damage
    {
        get { return unitData.GetAttribute(AttributeType.Damage).GetValue(); }
    }

    public float ArmorIgnore
    {
        get { return unitData.GetAttribute(AttributeType.ArmorIgnore).GetValue(); }
    }

    public float Reach
    {
        get { return unitData.GetAttribute(AttributeType.Reach).GetValue(); }
    }

    public float MoveSpeed
    {
        get { return unitData.GetAttribute(AttributeType.MoveSpeed).GetValue(); }
    }

    public float RotationSpeed
    {
        get { return unitData.GetAttribute(AttributeType.RotationSpeed).GetValue(); }
    }

    public float ViewRange
    {
        get { return unitData.GetAttribute(AttributeType.ViewRange).GetValue(); }
    }
    #endregion

    #region other properties

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public UnitController ActiveController
    {
        get { return activeController; }
        set
        {
            activeController = value;
        }
    }

    #endregion

    void Awake () {
        damageReceived = 0;
        rotationSpeed = 2;
        isMoving = false;
        isRotating = false;
        isCasting = false;
        isDead = false;
        isStunned = false;

        if (skills == null)
            skills = new List<Skill>();

        if (perks == null)
            perks = new List<Perk>();

        graphics = gameObject.AddComponent<UnitGraphics>();

        if (unitDataEditor != null)
        {
            unitData = new UnitData(unitDataEditor);
        }
	}

    void Start()
    {
        applyPerks();
    }
	
	void Update () {

        float dTime = Time.deltaTime;

        if (alive() && !isStunned)
        {
            //Movement
            if (canMove())
            {
                Vector3 offset = moveDest - transform.position;

                if (MoveSpeed * dTime < offset.magnitude)
                {
                    offset *= MoveSpeed * dTime / offset.magnitude;
                }
                else
                {
                    isMoving = false;
                }

                transform.position += offset;

            }
            //Rotation
            if (isRotating)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, dTime * rotationSpeed * 360);
                if (transform.rotation == rotationTarget)
                {
                    isRotating = false;
                }
            }
            //Skill Casting
            if (isCasting && canReachCastTarget())
            {
                currentCastTime += dTime;
                if (currentCastTime >= activeSkill.CastTime)
                {
                    activeSkill.Cast(this, castTarget);
                    isCasting = false;
                    if(castEvent != null) castEvent(this, new CastEventArgs(activeSkill));
                }
            }
            else
            {
                resetCast();
            }
        }
        //Stun
        else if (isStunned)
        {
            stunTime -= dTime;
            if(stunTime<= 0)
            {
                resetStun();
            }
        }
   
    }

    public void Cast(Skill skill, Skill.TargetInfo target)
    {
        if (isCasting && activeSkill == skill && castTarget == target) return;
        isMoving = false;
        if(skill.CurrentCooldown == 0)
        {
            isCasting = true;
            activeSkill = skill;
            castTarget = target;
            currentCastTime = 0;
        }
    }

    public void moveTo(Vector3 moveDest)
    {
        if (moveDest != transform.position)
        {
            this.moveDest = moveDest;
            rotationTarget = Quaternion.LookRotation(this.moveDest - transform.position);
            isMoving = true;
            isRotating = true;
        }
    }

    public void addPerk(Perk perk)
    {
        perk.Apply(this);
        perks.Add(perk);
    }

    public bool alive()
    {
        return !isDead;
    }

    void applyPerks()
    {
        foreach(Perk perk in perks)
        {
            perk.Apply(this);
        }
    }

    bool canMove()
    {
        return !isCasting && isMoving && !isDead;
    }

    public bool canReachCastTarget(Skill skill, Skill.TargetInfo target)
    {
        float totalReach = Reach + Size + skill.Reach;
        if (target.TargetUnit != null) totalReach += target.TargetUnit.Size;

        return
            (target.Position - transform.position).magnitude
            <=
            totalReach;
    }

    bool canReachCastTarget()
    {
        return canReachCastTarget(activeSkill, castTarget);
    }

    public void receiveDamage(float damage, Unit attacker)
    {
        float actualDamage = Mathf.Max(damage - Mathf.Max(Armor - attacker.ArmorIgnore, 0), 0);
        damageReceived += actualDamage;
        if(actualDamage > 0) lastAttacker = attacker;
        graphics.showDamage(actualDamage);
        if(damageReceived >= Hp)
        {
            die();
        }
    }

    public void removePerk(Perk perk)
    {
        perks.Remove(perk);
        perk.unapply(this);
    }

    public void resetCast()
    {
        isCasting = false;
        activeSkill = null;
        currentCastTime = 0;
        castTarget = null;
    }

    public void resetStun()
    {
        isStunned = false;
        stunTime = 0;
    }

    public void stun(float time)
    {
        isStunned = true;
        stunTime += time;
        graphics.showMessage("Stunned!");
    }

    public void die()
    {
        if (isDead) return;
        isDead = true;
        if(lastAttacker != null)
        {
            Xp xp = lastAttacker.gameObject.GetComponent<Xp>();
            if (xp != null)
            {
                xp.receiveXp((1000 * unitData.Level));
            }
        }
    }

    public float healthPercentage()
    {
        return 1f - (damageReceived / Hp);
    }
}
