using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public abstract partial class Skill : MonoBehaviour, IUniqueName {

    [SerializeField] protected SkillData data;
    protected Unit unit;
    private float currentCooldown;
    private int comboCounter;
    private float speedModifier;

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

    public void Initialize(SkillData skillData, Unit unit)
    {
        data = new SkillData(skillData);
        this.unit = unit;
        OnInitialize();
        speedModifier = GetSpeedModifier(unit);
    }

    protected virtual void OnInitialize() { }

    public void Cast(Unit caster, TargetInfo targetInfo)
    {
        Cast(caster, Targeter.GetTargets(this, targetInfo));
    }

    public void Cast(Unit caster, List<CastTarget> targets)
    {
        currentCooldown = Cooldown;
        CastInternal(caster, targets);
    }

    public virtual CanReachTargetResult CanReachTarget(TargetInfo targetInfo)
    {
        //debug
        //bool isPlayer = targetInfo.Caster.name == "Player";
        //if(isPlayer) Debug.Log("I am player");

        Unit caster = targetInfo.Caster;
        if (caster != null)
        {
            float totalReach = GetReach(caster);
            //if (isPlayer) Debug.LogFormat("My total reach is {0}", totalReach);

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

    public virtual void RegisterCombo(int counter)
    {
        comboCounter = counter;
        if(MaxCombo > 0) comboCounter %= MaxCombo;
    }

    public void ModifyAttribute(SkillData.AttributeType type, float bonus, float multiplier)
    {
        Data.GetAttribute(type).Modify(bonus, multiplier);
    }

    public virtual float GetReach(Unit caster)
    {
        //Debug.LogFormat("Calling Skill.GetReach() - caster.Reach: {0}, caster.Size: {1}, Reach: {2}", caster.Reach, caster.Size, Reach);
        return caster.Reach + caster.Size + Reach;
    }

    public virtual float GetSpeedModifier(Unit unit)
    {
        return Data.NaturalSpeedModifier;
    }

    public bool HasNextCombo => comboCounter < MaxCombo;
    public virtual int MaxCombo => 0;
    public SkillData Data => data;
    public float CurrentCooldown => currentCooldown;
    public int ComboCounter => comboCounter;
    public string UniqueName => name;
    public TargetType Type => Data.TargetType;
    public float Reach => Data.Reach;
    public float ReachAngle => Data.ReachAngle;
    public float Cooldown => Data.Cooldown;
    public float CastTime => Data.CastTime / speedModifier;
    public float TotalCastTime => Data.TotalCastTime / speedModifier;
    public float EarlyBreakTime => Data.EarlyBreakTime / speedModifier;
    public SkillGraphics Graphics => Data.Graphics;
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
