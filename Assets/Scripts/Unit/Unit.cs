using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]
    private UnitData unitData;

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

    private new CapsuleCollider collider;
    private new Rigidbody rigidbody;

    void Awake () {
        damageReceived = 0;
        
        //Add some physics
        //collider = gameObject.AddComponent<CapsuleCollider>();
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rigidbody.drag = 10;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        //rigidbody.maxDepenetrationVelocity = 20;
        rigidbody.useGravity = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        if (GetComponentInChildren<Collider>() == null)
        {
            collider = gameObject.AddComponent<CapsuleCollider>();
        }

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

        if (unitData == null)
        {
            unitData = new UnitData();
        }        
    }

    void Start()
    {
        ApplyPerks();

        rigidbody.mass = Mathf.Sqrt(unitData.Height * Size * Size) * 4 * 100;

        if (collider != null)
        {
            collider.radius = Size;
            collider.height = unitData.Height - 2 * Size;
        }
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

    #region attribute properties
    public float Size
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.Size).Value / 2f; }
    }

    public float Hp
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.Hp).Value; }
    }

    public float Armor
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.Armor).Value; }
    }

    public float Regen
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.Regen).Value; }
    }

    public float Damage
    {
        get { return 10; } //TODO
    }

    public float ArmorIgnore
    {
        get { return 0; } //TODO
    }

    public float Reach
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.Reach).Value; }
    }

    public float MoveSpeed
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.MoveSpeed).Value; }
    }

    public float RotationSpeed
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.RotationSpeed).Value; }
    }

    public float ViewRange
    {
        get { return unitData.GetAttribute(UnitAttributes.Type.ViewRange).Value; }
    }
    #endregion
    
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

    public UnitData UnitData
    {
        get { return unitData; }
        set { unitData = value; }
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

    public bool Alive
    {
        get { return !deadState.IsActive; }
    }

    public float HealthPercentage
    {
        get { return 1f - (damageReceived / Hp); }
    }

    public float CurrentHp
    {
        get { return Hp - damageReceived; }
    }

    public Rigidbody Rigidbody
    {
        get { return rigidbody; }
    }
}
