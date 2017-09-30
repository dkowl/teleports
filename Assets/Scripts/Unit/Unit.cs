using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum AttributeType
    {
        Size,
        Hp,
        Armor,
        Regen,
        Damage,
        ArmorIgnore,
        Reach,
        MoveSpeed,
        ViewRange,
        RotationSpeed,
        Count
    }

    public UnitDataEditor unitDataEditor;
    public UnitData unitData;

    private List<ActionState> actionStates;
    private MovingState movingState;
    private RotatingState rotatingState;
    private CastingState castingState;
    private StunnedState stunnedState;
    private DeadState deadState;
    
    private float damageReceived;

    public List<Skill> skills;
    public List<Perk> perks;
    
    private UnitGraphics graphics;
    
    private UnitController activeController;

    #region attribute properties
    public float Size
    {
        get { return unitData.GetAttribute(AttributeType.Size).GetValue() / 2f; }
    }

    public float Hp
    {
        get { return unitData.GetAttribute(AttributeType.Hp).GetValue(); }
    }

    public float Armor
    {
        get { return unitData.GetAttribute(AttributeType.Armor).GetValue(); }
    }

    public float Regen
    {
        get { return unitData.GetAttribute(AttributeType.Regen).GetValue(); }
    }

    public float Damage
    {
        get { return unitData.GetAttribute(AttributeType.Damage).GetValue(); }
    }

    public float ArmorIgnore
    {
        get { return unitData.GetAttribute(AttributeType.ArmorIgnore).GetValue(); }
    }

    public float Reach
    {
        get { return unitData.GetAttribute(AttributeType.Reach).GetValue(); }
    }

    public float MoveSpeed
    {
        get { return unitData.GetAttribute(AttributeType.MoveSpeed).GetValue(); }
    }

    public float RotationSpeed
    {
        get { return unitData.GetAttribute(AttributeType.RotationSpeed).GetValue(); }
    }

    public float ViewRange
    {
        get { return unitData.GetAttribute(AttributeType.ViewRange).GetValue(); }
    }
    #endregion

    #region other properties

    public bool IsMoving
    {
        get { return movingState.IsActive; }
    }

    public UnitController ActiveController
    {
        get { return activeController; }
        set
        {
            activeController = value;
        }
    }

    public UnitGraphics Graphics
    {
        get { return graphics; }
    }

    public MovingState MovingState
    {
        get { return movingState; }
    }

    public RotatingState RotatingState
    {
        get { return rotatingState; }
    }

    public CastingState CastingState
    {
        get { return castingState; }
    }

    public DeadState DeadState
    {
        get { return deadState; }
    }

    public float HealthPercentage
    {
        get { return 1f - (damageReceived / Hp); }
    }

    public float CurrentHp
    {
        get { return Hp - damageReceived; }
    }
    #endregion

    void Awake () {
        damageReceived = 0;

        movingState = new MovingState(this);
        rotatingState = new RotatingState(this);
        castingState = new CastingState(this);
        stunnedState = new StunnedState(this);
        deadState = new DeadState(this);

        movingState.AddBlocker(castingState);
        movingState.AddBlocker(stunnedState);
        movingState.AddBlocker(deadState);
        
        rotatingState.AddBlocker(stunnedState);
        rotatingState.AddBlocker(deadState);

        castingState.AddBlocker(stunnedState);
        castingState.AddBlocker(deadState);

        stunnedState.AddBlocker(deadState);

        actionStates = new List<ActionState>();
        actionStates.Add(movingState);
        actionStates.Add(rotatingState);
        actionStates.Add(castingState);
        actionStates.Add(stunnedState);
        actionStates.Add(deadState);

        if (skills == null)
            skills = new List<Skill>();

        if (perks == null)
            perks = new List<Perk>();

        graphics = gameObject.AddComponent<UnitGraphics>();

        if(activeController == null)
        {
            activeController = gameObject.GetComponent<UnitController>();
        }

        if (unitDataEditor != null)
        {
            unitData = new UnitData(unitDataEditor);
        }
	}

    void Start()
    {
        ApplyPerks();
    }
	
	void Update () {

        float dTime = Time.deltaTime;

        foreach(ActionState state in actionStates)
        {
            state.Update(dTime);
        }   
    }

    public void CastStart(Skill skill, Skill.TargetInfo target)
    {
        Debug.Log("Trying to cast" + skill.Name + " on " + target.TargetUnit.name);
        castingState.ActiveSkill = skill;
        castingState.CastTarget = target;
        castingState.Start();
    }

    public void MoveStart(Vector3 moveDest)
    {
        movingState.MoveDest = moveDest;
    }

    public void AddPerk(Perk perk)
    {
        perk.Apply(this);
        perks.Add(perk);
    }

    public bool Alive()
    {
        return !deadState.IsActive;
    }

    void ApplyPerks()
    {
        foreach(Perk perk in perks)
        {
            perk.Apply(this);
        }
    }

    public void ReceiveDamage(float damage, Unit attacker)
    {
        float actualDamage = Mathf.Max(damage - Mathf.Max(Armor - attacker.ArmorIgnore, 0), 0);
        damageReceived += actualDamage;
        if(actualDamage > 0) deadState.LastAttacker = attacker;
        graphics.showDamage(actualDamage);
        if(damageReceived >= Hp)
        {
            deadState.Start();
        }
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perk.unapply(this);
    }

    public void Stun(float time)
    {
        stunnedState.StunTime = time;
    }
}
