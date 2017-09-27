using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Skill {

    public Attack attack_;
    public Perk perk_;

    Vector3 targetStartPosition_;
    UnitController oldController_;
    ChargeController chargeController_;

    public override void InternalCast(Unit caster, TargetInfo target) {
        
        oldController_ = caster.ActiveController;
        targetStartPosition_ = target.TargetUnit.gameObject.transform.position;

        chargeController_ = caster.gameObject.AddComponent<ChargeController>();
        chargeController_.Target = target;
        chargeController_.initialize(this);
    }

    public class ChargeController : UnitController
    {
        public Charge charge_;

        public void initialize(Charge charge)
        {
            charge_ = charge;
            unit.ActiveController = this;
            mainAttack = charge_.attack_;
            unit.addPerk(charge_.perk_);
        }

        void finalize()
        {
            unit.removePerk(charge_.perk_);
            unit.ActiveController = charge_.oldController_;
        }

        public override void Control()
        {
            if (unit.canReachCastTarget(mainAttack, target))
            {
                unit.Cast(mainAttack, target);
                finalize();
            }
            else
            {
                unit.moveTo(charge_.targetStartPosition_);
                if (unit.transform.position == charge_.targetStartPosition_) finalize();
            }
        }
    }
}
