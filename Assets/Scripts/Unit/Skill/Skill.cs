using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public abstract class Skill : MonoBehaviour, IUniqueName {
       
    [FormerlySerializedAs("name_"), SerializeField] new private string name;
    [FormerlySerializedAs("type_"), SerializeField] private TargetType type;
    [FormerlySerializedAs("reach_"), SerializeField] private Attribute reach;
    [FormerlySerializedAs("reach_"), SerializeField] private Attribute reachAngle = new Attribute(30);
    [FormerlySerializedAs("cooldown_"), SerializeField] private Attribute cooldown;
    [FormerlySerializedAs("castTime_"), SerializeField] private Attribute castTime;
    [SerializeField] private Attribute totalCastTime;
    [SerializeField] private Attribute earlyBreakTime;
    [SerializeField] private int maxCombo;
    [SerializeField] private SkillGraphics graphics;

    [SerializeField] private SkillData data;

    float currentCooldown;

    void OnEnable()
    {
        Debug.Log("OnEnable called");
    }

    //temporary method to refactor assets
    [Button]
    public void PopulateSkillData()
    {
        data.PopulateFromSkill(this);
    }

    public virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0) currentCooldown = 0;
    }
    
    public abstract void CastInternal(Unit caster, List<CastTarget> targets);

    protected virtual SkillTargeter GetTargeter()
    {
        return new SkillTargeter_Point();
    }

    public void Cast(Unit caster, TargetInfo targetInfo)
    {
        currentCooldown = cooldown.Value;
        CastInternal(caster, Targeter.GetTargets(this, targetInfo));
    }

    public CanReachTargetResult CanReachTarget(TargetInfo targetInfo)
    {
        Unit caster = targetInfo.Caster;
        if (caster != null)
        {
            float totalReach = GetReach(caster);
            if (targetInfo.TargetUnit != null) totalReach += targetInfo.TargetUnit.Size;

            bool canReachDistance = (targetInfo.Position - caster.transform.position).magnitude <= totalReach;

            float casterYRot = caster.transform.rotation.eulerAngles.y;
            float targetYRot = Quaternion.LookRotation(targetInfo.Position - caster.transform.position).eulerAngles.y;
            bool canReachAngle = Mathf.Abs(Mathf.DeltaAngle(casterYRot, targetYRot)) <= ReachAngle / 2f;

            if (canReachAngle && canReachDistance) return CanReachTargetResult.Yes;
            else
            {
                CanReachTargetResult result = 0;
                if (!canReachDistance) result += CanReachTargetResult.NoDistance;
                if (!canReachAngle) result += CanReachTargetResult.NoAngle;
                return result;
            }
        }
        else
        {
            return CanReachTargetResult.NoAngle + CanReachTargetResult.NoDistance;
        }
    }

    public void ModifyAttribute(SkillData.AttributeType type, float bonus, float multiplier)
    {
        Data.GetAttribute(type).Modify(bonus, multiplier);
    }

    Attribute GetAttribute(SkillData.AttributeType type)
    {
        return Data.GetAttribute(type);
    }

    public virtual float GetReach(Unit caster)
    {
        return caster.Reach + caster.Size + Reach;
    }

    public string UniqueName
    {
        get { return name; }
    }

    public TargetType Type
    {
        get { return type; }
    }

    public float Reach
    {
        get { return reach.Value; }
    }

    public float ReachAngle
    {
        get { return reachAngle.Value; }
    }

    public float Cooldown
    {
        get { return cooldown.Value; }
    }

    public float CastTime
    {
        get { return castTime.Value; }
    }

    public float TotalCastTime
    {
        get { return totalCastTime.Value; }
    }

    public float EarlyBreakTime
    {
        get { return earlyBreakTime.Value; }
    }

    public int MaxCombo
    {
        get { return maxCombo; }
    }

    public float CurrentCooldown
    {
        get { return currentCooldown; }
    }

    public SkillGraphics Graphics
    {
        get { return graphics; }
    }

    public SkillData Data { get { return data; } }

    private SkillTargeter Targeter
    {
        get
        {
            return GetTargeter();
        }
    }

    public enum TargetType
    {
        Unit,
        Position
    };

    public struct CanReachTargetResult
    {
        private int value;

        public static readonly CanReachTargetResult Yes = 1;
        public static readonly CanReachTargetResult NoDistance = 2;
        public static readonly CanReachTargetResult NoAngle = 4;

        private CanReachTargetResult(int value) { this.value = value; }

        public static implicit operator bool(CanReachTargetResult a)
        {
            if (a == Yes) return true;
            else return false;
        }

        public static implicit operator int (CanReachTargetResult a)
        {
            return a.value;
        }

        public static implicit operator CanReachTargetResult(int value)
        {
            return new CanReachTargetResult(value);
        }

        public static bool operator==(CanReachTargetResult a, CanReachTargetResult b)
        {
            return a.value == b.value;
        }

        public static bool operator !=(CanReachTargetResult a, CanReachTargetResult b)
        {
            return !(a == b);
        }
    }

    [System.Serializable]
    public class TargetInfo
    {
        Unit caster;
        TargetType targetType;
        Unit targetUnit;
        Vector3 targetPosition;

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

        public Unit Caster
        {
            get { return caster; }
        }

        public TargetType TargetType
        {
            get { return targetType; }
        }

        public Unit TargetUnit
        {
            get { return targetUnit; }
            set { targetUnit = value; }
        }

        public Vector3 Position
        {
            get
            {
                if (targetUnit != null)
                {
                    return TargetUnit.transform.position;
                }
                else return targetPosition;
            }
        }
    }
}
