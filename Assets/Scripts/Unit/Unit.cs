using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]
    private UnitData unitData;
    private UnitWeaponCombiner weaponCombiner;

    private List<ActionState> actionStates;
    private MovingState movingState;
    private RotatingState rotatingState;
    private CastingState castingState;
    private StunnedState stunnedState;
    private DeadState deadState;
    
    private float damageReceived;

    private List<Skill> skills;
    private List<Perk> perks;
    
    private UnitGraphics graphics;
    private UnitController activeController;
    private UnitPhysics physics;
    

    void Awake () {
        damageReceived = 0;

        movingState = new MovingState(this);
        rotatingState = new RotatingState(this);
        castingState = new CastingState(this);
        stunnedState = new StunnedState(this);
        deadState = new DeadState(this);

        movingState.AddPauser(castingState);
        movingState.AddPauser(stunnedState);
        movingState.AddBlocker(deadState);
        
        rotatingState.AddPauser(stunnedState);
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

        if (unitData == null)
        {
            unitData = new UnitData();
        }        
    }

    void Start()
    {
        ApplyPerks();
        physics = gameObject.AddComponent<UnitPhysics>();
    }
	
	void Update () {

        float dTime = Time.deltaTime;

        foreach(ActionState state in actionStates)
        {
           if(!(state is MovingState || state is RotatingState))
                state.Update(dTime);
        }   
    }

    void FixedUpdate()
    {
        rotatingState.Update(Time.deltaTime);
        movingState.Update(Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Size + Reach);
    }

    public void AddPerk(Perk perk)
    {
        perk.Apply(this);
        perks.Add(perk);
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
            deadState.StartDeath();
        }
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perk.Unapply(this);
    }

    public void Stun(float time)
    {
        stunnedState.StunTime = time;
    }

    public void Kill()
    {
        deadState.StartDeath();
    }
    
    public float Size => unitData.GetAttribute(UnitAttributesData.AttributeType.Size).Value;
    public float Hp => unitData.GetAttribute(UnitAttributesData.AttributeType.HealthPoints).Value;
    public float Armor => unitData.GetAttribute(UnitAttributesData.AttributeType.Armor).Value;
    public float Regen => unitData.GetAttribute(UnitAttributesData.AttributeType.Regeneration).Value;
    public float Damage => weaponCombiner.DamageRoll;
    public float ArmorIgnore => 0;
    public float Reach => unitData.GetAttribute(UnitAttributesData.AttributeType.Reach).Value;
    public float MoveSpeed => unitData.GetAttribute(UnitAttributesData.AttributeType.MovementSpeed).Value;
    public float RotationSpeed => unitData.GetAttribute(UnitAttributesData.AttributeType.RotationSpeed).Value;
    public float ViewRange => unitData.GetAttribute(UnitAttributesData.AttributeType.ViewRange).Value;
    
    public bool IsMoving => movingState.IsActive;
    public UnitController ActiveController
    {
        get { return activeController; }
        set
        {
            activeController = value;
        }
    }
    public UnitData UnitData
    {
        get { return unitData; }
        set {
            unitData = value;
            weaponCombiner = new UnitWeaponCombiner(unitData, WeaponData);
        }
    }
    public UnitWeaponCombiner WeaponCombiner => weaponCombiner;
    public MovingState MovingState => movingState;
    public RotatingState RotatingState => rotatingState;
    public CastingState CastingState => castingState;
    public DeadState DeadState => deadState;
    public bool Alive => !deadState.IsActive;
    public float HealthPercentage => 1f - damageReceived / Hp;
    public float CurrentHp => Hp - damageReceived;
    public List<Perk> Perks => perks;
    public List<Skill> Skills => skills;
    public Skill PrimarySkill => skills.Count > 0 ? skills[0] : null;
    public UnitGraphics Graphics => graphics;
    public UnitPhysics Physics => physics;
    private WeaponData WeaponData{
        get
        {
            foreach(var itemInfo in UnitData.Inventory.EquipmentData.GetEquippedItems())
            {
                if (itemInfo.Item.IsType(ItemType.Weapon)) return itemInfo.Item.WeaponData;
            }
            return null;
        }
    }
}
