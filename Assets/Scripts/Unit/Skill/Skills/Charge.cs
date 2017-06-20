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

    public override void internalCast(Unit caster, TargetInfo target) {
        
        oldController_ = caster.activeController_;
        targetStartPosition_ = target.unit.gameObject.transform.position;

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
            unit_.activeController_ = this;
            mainAttack_ = charge_.attack_;
            unit_.addPerk(charge_.perk_);
        }

        void finalize()
        {
            unit_.removePerk(charge_.perk_);
            unit_.activeController_ = charge_.oldController_;
        }

        public override void control()
        {
            if (unit_.canReachCastTarget(mainAttack_, target_))
            {
                unit_.cast(mainAttack_, target_);
                finalize();
            }
            else
            {
                unit_.moveTo(charge_.targetStartPosition_);
                if (unit_.transform.position == charge_.targetStartPosition_) finalize();
            }
        }
    }
}
