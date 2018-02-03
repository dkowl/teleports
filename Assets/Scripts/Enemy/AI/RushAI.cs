using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class RushAI : UnitController {


    GameObject[] targets; //all available targets
    List<Skill> attacks;
    
	void Start () {
        FindTarget();
        Subscribe();
	}

    void OnDestroy()
    {
        Unsubscribe();
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
        TargetInfo bestTarget = BestTarget;

        if (bestTarget != null && bestTarget.Distance < unit.ViewRange)
        {
            target.TargetUnit = targets[bestTarget.Id].GetComponent<Unit>();
            RandomizeAttack();
        }
        else target.TargetUnit = null;
    }

    protected void RandomizeAttack()
    {
        Debug.Log("Randomizing attacks");
        if (attacks.Count > 0)
        {
            mainAttack = attacks.RandomElement();
            Debug.Log("Attack randomized to " + mainAttack.name);
        }
    }

    protected void OnCastReset(CastingState.CastEventArgs eventArgs)
    {
        RandomizeAttack();
    }

    private void Subscribe()
    {
        Unsubscribe();
        unit.CastingState.resetCastEvent += OnCastReset;
    }

    private void Unsubscribe()
    {
        unit.CastingState.resetCastEvent -= OnCastReset;
    }

    public List<Skill> Attacks
    {
        set { attacks = value; }
    }

    protected TargetInfo BestTarget
    {
        get
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
            //select closest player
            float minDist = float.MaxValue;

            int bestArg = 0;
            bool found = false;
            for (int i = 0; i < targets.Length; i++)
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
            if (found) return new TargetInfo(minDist, bestArg);
            else return null;
        }
    }

    public class TargetInfo
    {
        private float distance;
        private int id;

        public TargetInfo(float distance, int id)
        {
            this.distance = distance;
            this.id = id;
        }

        public float Distance { get { return distance; } }
        public int Id { get { return id; } }
    }
}
