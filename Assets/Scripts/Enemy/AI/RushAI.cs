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
        else Chase();
	}
    
    void FindTarget()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        //select closest player
        float minDist = float.MaxValue;

        int bestArg = 0;
        for (int i = 0; i<targets.Length; i++)
        {
            float dist = Vector3.Distance(targets[i].transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                bestArg = i;
            }
        }

        if (minDist < unit.ViewRange)
        {
            target.TargetUnit = targets[bestArg].GetComponent<Unit>();
        }
        else target.TargetUnit = null;
    }
}
