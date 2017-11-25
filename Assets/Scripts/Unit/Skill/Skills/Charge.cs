using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Skill {

    public Attack attack;
    public Perk perk;

    Vector3 targetStartPosition;
    UnitController oldController;
    ChargeController chargeController;

    public override void InternalCast(Unit caster, TargetInfo target) {
        
        oldController = caster.ActiveController;
        targetStartPosition = target.TargetUnit.gameObject.transform.position;

        chargeController = caster.gameObject.AddComponent<ChargeController>();
        chargeController.Target = target;
        chargeController.Initialize(this);
    }

    public class ChargeController : UnitController
    {
        public Charge charge;

        public void Initialize(Charge charge)
        {
            this.charge = charge;
            unit.ActiveController = this;
            mainAttack = this.charge.attack;
            unit.AddPerk(this.charge.perk);
        }

        void Finish()
        {
            unit.RemovePerk(charge.perk);
            unit.ActiveController = charge.oldController;
        }

        public override void Control()
        {
            if (mainAttack.CanReachCastTarget(target))
            {
                unit.CastingState.Start(mainAttack, target);
                Finish();
            }
            else
            {
                unit.MovingState.Start(charge.targetStartPosition);
                if (unit.transform.position == charge.targetStartPosition) Finish();
            }
        }
    }
}
