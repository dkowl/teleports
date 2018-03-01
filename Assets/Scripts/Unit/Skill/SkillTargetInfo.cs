using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill{

    [System.Serializable]
    public class TargetInfo
    {
        private Unit caster;
        private TargetType targetType;
        private Unit targetUnit;
        private Vector3 targetPosition;

        public TargetInfo(Unit caster, Unit targetUnit)
            : this(caster, TargetType.Unit, targetUnit, Vector3.zero)
        { }

        public TargetInfo(Unit caster, Vector3 targetPosition)
            : this(caster, TargetType.Position, null, targetPosition)
        { }

        TargetInfo(Unit caster, TargetType targetType, Unit targetUnit, Vector3 targetPosition)
        {
            this.caster = caster;
            this.targetType = targetType;
            this.targetUnit = targetUnit;
            this.targetPosition = targetPosition;
        }

        public TargetInfo(TargetInfo other)
        {
            this.caster = other.caster;
            this.targetType = other.targetType;
            this.targetUnit = other.targetUnit;
            this.targetPosition = other.targetPosition;
        }

        public override bool Equals(object obj)
        {
            TargetInfo other = (TargetInfo)obj;
            if (obj == null) return false;
            else
            {
                return
                    caster == other.caster &&
                    targetType == other.targetType &&
                    targetUnit == other.targetUnit &&
                    targetPosition == other.targetPosition;
            }
        }

        public override int GetHashCode()
        {
            return
                caster.GetHashCode() ^
                targetType.GetHashCode() ^
                targetUnit.GetHashCode() ^
                targetPosition.GetHashCode();
        }

        public Unit Caster => caster;
        public TargetType TargetType => targetType;
        public Unit TargetUnit
        {
            get { return targetUnit; }
            set { targetUnit = value; }
        }
        public Vector3 Position => targetUnit != null ? TargetUnit.transform.position : targetPosition;
    }
}
