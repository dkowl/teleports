using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    protected Unit unit;
    protected Skill.TargetInfo target;
    [SerializeField] protected Skill mainAttack;

    public virtual void Awake()
    {
        unit = gameObject.GetComponent<Unit>();
        target = new Skill.TargetInfo(unit, null);
        if(mainAttack == null)
        {
            mainAttack = unit.PrimarySkill;
        }
        if(unit.ActiveController == null)
        {
            unit.ActiveController = this;
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            Control();
        }
    }

    public abstract void Control();

    protected void Chase()
    {
        if (mainAttack.CanReachTarget(target))
        {
            unit.CastingState.TryStart(mainAttack, target);
        }
        else
        {
            unit.MovingState.Start(target.Position);
        }

        unit.RotatingState.RotationTarget = Quaternion.LookRotation(target.Position - unit.transform.position);
    }

    private bool IsActive
    {
        get { return this == unit.ActiveController; }
    }

    public Skill.TargetInfo Target
    {
        get { return target; }
        set { target = value; }
    }

    public Skill MainAttack
    {
        get { return mainAttack; }
        set { mainAttack = value; }
    }
}
