using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int hp_, damage_;
    public float attackRange_, attackCooldown_, moveSpeed_;

    //pathfinding
    public Vector3 moveDest_;
    bool isMoving_;

	// Use this for initialization
	void Start () {
        hp_ = 100;
        damage_ = 25;
        attackRange_ = 1;
        attackCooldown_ = 1;
        moveSpeed_ = 1;
        isMoving_ = false;
	}
	
	// Update is called once per frame
	void Update () {

        float dTime = Time.deltaTime;

        if (isMoving_)
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
	}

    public void moveTo(Vector3 moveDest)
    {
        moveDest_ = moveDest;
        isMoving_ = true;
    }
}
