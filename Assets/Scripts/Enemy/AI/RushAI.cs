using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : MonoBehaviour {


    GameObject[] targets_; //all available targets
    GameObject target_; //chosen target
    Unit targetUnit_, unit_;

    
    //Unity Event Functions
	void Awake()
    {
        targets_ = GameObject.FindGameObjectsWithTag("Player");
        unit_ = gameObject.GetComponent<Unit>();
    }

	void Start () {
        target();
	}
	
	void Update () {
        if (targetUnit_ == null || !targetUnit_.alive()) target();
	}

    //goal: find target_
    void findTarget()
    {
        //select closest player
        float minDist = float.MaxValue;

        int bestArg = 0;
        for (int i = 0; i<targets_.Length; i++)
        {
            float dist = Vector3.Distance(targets_[i].transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                bestArg = i;
            }
        }

        target_ = targets_[bestArg];
        targetUnit_ = target_.GetComponent<Unit>();
        unit_.AttackTarget = targetUnit_;
    }

    //goal: assign unit_.AttackTarget
    void lockTarget()
    {
        targetUnit_ = target_.GetComponent<Unit>();
        unit_.AttackTarget = targetUnit_;
    }

    void target()
    {
        findTarget();
        lockTarget();
    }
}
