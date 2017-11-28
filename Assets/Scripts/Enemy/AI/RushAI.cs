using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAI : UnitController {


    GameObject[] targets; //all available targets
    
    //Unity Event Functions
	public override void Awake()
    {
        base.Awake();
        mainAttack = gameObject.GetComponent<Attack>();
    }

	void Start () {
        FindTarget();
	}

    public override void Control() {
        if (target.TargetUnit == null) FindTarget();
        else
        {
            if (target.TargetUnit.Alive)
            {
                Chase();
            }
            else
            {
                target.TargetUnit = null;
            }
        }
	}
    
    void FindTarget()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        //select closest player
        float minDist = float.MaxValue;

        int bestArg = 0;
        bool found = false;
        for (int i = 0; i<targets.Length; i++)
        {
            if (targets[i].GetComponent<Unit>() != null && !targets[i].GetComponent<Unit>().Alive) continue;

            float dist = Vector3.Distance(targets[i].transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                bestArg = i;
                found = true;
            }
        }

        if (found && minDist < unit.ViewRange)
        {
            target.TargetUnit = targets[bestArg].GetComponent<Unit>();
        }
        else target.TargetUnit = null;
    }
}
