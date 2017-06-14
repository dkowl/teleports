using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : MonoBehaviour {


    GameObject[] targets_; //all available targets
    Unit unit_;
    public Skill skill_;
    Skill.TargetInfo target_;

    
    //Unity Event Functions
	void Awake()
    {
        targets_ = GameObject.FindGameObjectsWithTag("Player");
        target_ = new Skill.TargetInfo();
        unit_ = gameObject.GetComponent<Unit>();
    }

	void Start () {
        target();
	}
	
	void Update () {
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

    void chase()
    {
        if(unit_.canReachCastTarget(skill_, target_))
        {
            unit_.cast(skill_, target_);
        }
        else
        {
            unit_.moveTo(target_.position);
        }
    }
}
