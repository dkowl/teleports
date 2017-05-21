using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour {

    GameObject[] targets_;
    GameObject target_;
    Unit targetUnit_, unit_;

	// Use this for initialization
	void Start () {
        targets_ = GameObject.FindGameObjectsWithTag("Player");
        unit_ = gameObject.GetComponent<Unit>();
        target();
	}
	
	// Update is called once per frame
	void Update () {
        if (targetUnit_ == null) target();
	}

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

        target_ = targets_[bestArg];
        targetUnit_ = target_.GetComponent<Unit>();
        unit_.AttackTarget = targetUnit_;
    }
}
