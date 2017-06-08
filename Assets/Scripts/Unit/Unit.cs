using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    string name_;
    public int level_;

    //basic combat stats
    public int hp_, damage_;
    public float
        attackRange_, attackCooldown_, attackTime_,
        moveSpeed_, viewRange_;
    float rotationSpeed_;

    //hp
    int damageReceived_;
    bool isDead_;

    //collision
    public float
        innerRadius_,
        heigth_;

    //pathfinding
    Vector3 moveDest_;
    bool isMoving_;

    Quaternion rotationTarget_;
    bool isRotating_;

    //enemy targeting
    Unit attackTarget_;
    public Unit AttackTarget {
        get
        {
            return attackTarget_;
        }
        set
        {
            attackTarget_ = value;
        }
    }
    float
        currentAttackCooldown_,
        currentAttackTime_;
    bool isAttacking_;

    //kill rewarding
    Unit lastAttacker_;

    //graphics
    UnitGraphics graphics_;

    // Use this for initialization
    void Start () {
        damageReceived_ = 0;
        rotationSpeed_ = 2;
        isMoving_ = false;
        isRotating_ = false;
        isAttacking_ = false;
        isDead_ = false;

        graphics_ = gameObject.AddComponent<UnitGraphics>();
	}
	
	// Update is called once per frame
	void Update () {

        float dTime = Time.deltaTime;

        if (alive())
        {
            if (canMove())
            {
                Vector3 offset = moveDest_ - transform.position;

                if (moveSpeed_ * dTime < offset.magnitude)
                {
                    offset *= moveSpeed_ * dTime / offset.magnitude;
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

            if (attackTarget_ != null)
            {
                if (!attackTarget_.alive()) resetAttack();
                else
                {
                    chase();
                }
            }

            if (isAttacking_) currentAttackTime_ += dTime;
            else currentAttackTime_ = 0;
        }

        currentAttackCooldown_ -= dTime;
        if (currentAttackCooldown_ < 0) currentAttackCooldown_ = 0;
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

    public void attack()
    {
        if (
            attackTarget_ != null &&
            currentAttackCooldown_ <= 0 &&
            canReachAttackTarget()
            )
        {
            isMoving_ = false;
            isAttacking_ = true;
            if (currentAttackTime_ >= attackTime_)
            {
                currentAttackCooldown_ = attackCooldown_;
                attackTarget_.receiveDamage(damage_, this);
                isAttacking_ = false;
            }
        }
    }

    public bool alive()
    {
        return !isDead_;
    }

    bool canMove()
    {
        return !isAttacking_ && isMoving_ && !isDead_;
    }

    bool canReachAttackTarget()
    {
        return 
            (attackTarget_.transform.position - transform.position).magnitude 
            <= 
            attackRange_ + innerRadius_ + attackTarget_.innerRadius_;
    }

    public void chase()
    {
        float dist = Vector3.Distance(attackTarget_.transform.position, transform.position);
        if (dist <= viewRange_)
        {
            if (canReachAttackTarget())
            {
                attack();
            }
            else
            {
                moveTo(attackTarget_.transform.position);
                isAttacking_ = false;
            }
        }
    }

    public void receiveDamage(int damage, Unit attacker)
    {
        damageReceived_ += damage;
        lastAttacker_ = attacker;
        graphics_.showDamage(damage);
        if(damageReceived_ >= hp_)
        {
            die();
        }
    }

    public void resetAttack()
    {
        isAttacking_ = false;
        currentAttackTime_ = 0;
        attackTarget_ = null;
    }

    public void die()
    {
        isDead_ = true;
        if(lastAttacker_ != null)
        {
            Xp xp = lastAttacker_.gameObject.GetComponent<Xp>();
            if (xp != null)
            {
                xp.receiveXp((int)(1000 * level_));
            }
        }
    }

    public float healthPercentage()
    {
        return 1f - (float)damageReceived_ / hp_;
    }
}
