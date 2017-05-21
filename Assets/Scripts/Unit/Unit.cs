using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int hp_, damage_;
    public float
        attackRange_, attackCooldown_,
        moveSpeed_, rotationSpeed_,
        viewRange_;

    //hp
    public int damageReceived_;

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
    float currentAttackCooldown_;

    // Use this for initialization
    void Start () {
        damageReceived_ = 0;
        isMoving_ = false;
        isRotating_ = false;
	}
	
	// Update is called once per frame
	void Update () {

        float dTime = Time.deltaTime;

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

        if (attackTarget_ != null) chase();

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
        if(attackTarget_ != null && currentAttackCooldown_ <= 0 && (attackTarget_.transform.position - transform.position).magnitude <= attackRange_)
        {
            currentAttackCooldown_ = attackCooldown_;
            attackTarget_.receiveDamage(damage_);
            isMoving_ = false;
        }
    }

    bool canMove()
    {
        return currentAttackCooldown_ == 0 && isMoving_;
    }

    public void chase()
    {
        float dist = Vector3.Distance(attackTarget_.transform.position, transform.position);
        if (dist <= viewRange_)
        {
            if (dist <= attackRange_)
            {
                attack();
            }
            else
            {
                moveTo(attackTarget_.transform.position);
            }
        }
    }

    public void receiveDamage(int damage)
    {
        damageReceived_ += damage;
        if(damageReceived_ >= hp_)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
