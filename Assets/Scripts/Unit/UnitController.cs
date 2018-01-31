using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    protected Unit unit;
    protected Skill.TargetInfo target;
    [SerializeField]
    protected Skill mainAttack;

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

    public virtual void Awake()
    {
        unit = gameObject.GetComponent<Unit>();
        target = new Skill.TargetInfo(unit, null);
        if(mainAttack == null)
        {
            mainAttack = MainData.CurrentGameData.GetSkill(unit.UnitData.MainAttack);
        }
        if(unit.ActiveController == null)
        {
            unit.ActiveController = this;
        }
    }

    void Update()
    {
        if (IsActive)
        {
            Control();
        }
    }

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
    }

    public abstract void Control();

    bool IsActive
    {
        get { return this == unit.ActiveController; }
    }
}
