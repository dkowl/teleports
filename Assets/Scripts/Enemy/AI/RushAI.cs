using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class RushAI : UnitController {

    private GameObject[] targets; //all available targets
    private List<Skill> attacks;

    private void Start () {
        FindTarget();
        Subscribe();
	}

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public override void Control() {
        if (target.TargetUnit == null) FindTarget();
        else
        {
            if (target.TargetUnit.Alive && IsWithinReach(target.TargetUnit.gameObject))
            {
                Chase();
            }
            else
            {
                target.TargetUnit = null;
            }
        }
	}

    private void FindTarget()
    {
        int bestTargetId = FindBestTargetId();

        if (IsWithinReach(targets[bestTargetId]))
        {
            target.TargetUnit = targets[bestTargetId].GetComponent<Unit>();
            RandomizeAttack();
        }
        else target.TargetUnit = null;
    }

    protected int FindBestTargetId()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");

        //select closest player
        float minDist = float.MaxValue;
        int? bestArg = null;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<Unit>() != null && !targets[i].GetComponent<Unit>().Alive) continue;

            float dist = DistanceToTarget(targets[i]);
            if (dist < minDist)
            {
                minDist = dist;
                bestArg = i;
            }
        }
        return bestArg ?? 0;
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

    private bool IsWithinReach(GameObject targetObject)
    {
        return DistanceToTarget(targetObject) < unit.ViewRange;
    }

    private float DistanceToTarget(GameObject targetObject)
    {
        return Vector3.Distance(targetObject.transform.position, transform.position);
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
}
