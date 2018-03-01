using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public abstract partial class Skill : MonoBehaviour, IUniqueName {

    [SerializeField] private SkillData data;
    private float currentCooldown;

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0) currentCooldown = 0;
    }
    
    protected abstract void CastInternal(Unit caster, List<CastTarget> targets);

    protected virtual SkillTargeter GetTargeter()
    {
        return new SkillTargeter_Point();
    }

    public void Cast(Unit caster, TargetInfo targetInfo)
    {
        currentCooldown = Cooldown;
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

    public virtual float GetReach(Unit caster)
    {
        return caster.Reach + caster.Size + Reach;
    }

    public SkillData Data
    {
        get { return data; }
        set { data = new SkillData(value); }
    }
    public string UniqueName => name;
    public TargetType Type => data.TargetType;
    public float Reach => data.Reach;
    public float ReachAngle => data.ReachAngle;
    public float Cooldown => data.Cooldown;
    public float CastTime => data.CastTime;
    public float TotalCastTime => data.TotalCastTime;
    public float EarlyBreakTime => data.EarlyBreakTime;
    public int MaxCombo => data.MaxCombo;
    public float CurrentCooldown => currentCooldown;
    public SkillGraphics Graphics => data.Graphics;
    private SkillTargeter Targeter => GetTargeter();

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
            return a == Yes;
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
}
