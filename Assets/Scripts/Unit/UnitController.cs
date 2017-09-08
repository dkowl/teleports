using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    protected Unit unit_;
    public Skill mainAttack_;
    protected Skill.TargetInfo target_;
    public Skill.TargetInfo Target
    {
        set { target_ = value; }
    }

    public virtual void Awake()
    {
        unit_ = gameObject.GetComponent<Unit>();
        target_ = new Skill.TargetInfo();
    }

    void Update()
    {
        if (isActive())
        {
            control();
        }
    }

    protected void chase()
    {
        if (unit_.canReachCastTarget(mainAttack_, target_))
        {
            unit_.cast(mainAttack_, target_);
        }
        else
        {
            unit_.moveTo(target_.Position);
        }
    }

    public abstract void control();

    bool isActive()
    {
        return this == unit_.activeController;
    }
}
