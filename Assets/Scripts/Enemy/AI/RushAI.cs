using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : UnitController {


    GameObject[] targets_; //all available targets
    
    //Unity Event Functions
	public override void Awake()
    {
        base.Awake();
        targets_ = GameObject.FindGameObjectsWithTag("Player");
        mainAttack_ = gameObject.GetComponent<Attack>();
    }

	void Start () {
        target();
	}

    public override void control() {
        if (target_.unit == null) target();
        else chase();
	}

    //goal: find target_
    void target()
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

        if (minDist < unit_.ViewRange)
        {
            target_.unit = targets_[bestArg].GetComponent<Unit>();
        }
        else target_.unit = null;
    }
}
