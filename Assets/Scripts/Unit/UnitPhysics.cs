using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[RequireComponent(typeof(Unit))]
public class UnitPhysics : MonoBehaviour
{
    private Unit unit;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private UnitRagdoll ragdoll;
    private bool isRagdoll;

    [SerializeField] private float defaultForce = 1000;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        SpawnRigidbody();
        collider = SpawnCollider();
    }

    private void SpawnRigidbody()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rigidbody.drag = 10;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbody.useGravity = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        rigidbody.mass = Mathf.Sqrt(unit.UnitData.Height * unit.Size * unit.Size) * 4 * 10;
    }

    private Collider FindExistingCollider()
    {
        Collider result = null;

        result = unit.GetComponent<Collider>();
        if(result != null) return result;

        RaceID raceId = new RaceID(unit.UnitData.RaceName);
        result = Main.StaticData.Game.Races.GetValue(raceId).Graphics.Collider;
        if (result != null) return gameObject.AddCopyOfComponent(result);

        //the collider must be in the same gameObject as the rigidbody, so if we find one in a child object, we move it up
        Collider childCollider = unit.GetComponentInChildren<Collider>();
        if (childCollider is BoxCollider) result = gameObject.AddCopyOfComponent(childCollider as BoxCollider);
        if (childCollider is SphereCollider) result = gameObject.AddCopyOfComponent(childCollider as SphereCollider);
        if (childCollider is CapsuleCollider) result = gameObject.AddCopyOfComponent(childCollider as CapsuleCollider);
        if (childCollider is MeshCollider) result = gameObject.AddCopyOfComponent(childCollider as MeshCollider);

        Destroy(childCollider);
        return result;
    }

    private Collider SpawnCollider()
    {
        var foundCollider = FindExistingCollider();
        if (foundCollider != null)
        {
            return foundCollider;
        }

        CapsuleCollider result = gameObject.AddComponent<CapsuleCollider>();
        result.radius = unit.Size;
        result.height = unit.UnitData.Height + unit.Size;
        result.center = new Vector3(0, unit.UnitData.Height / 2, 0);
        return result;
    }

    public void SwitchToRagdoll()
    {
        unit.gameObject.SetLayerIncludingChildren(0);

        var ragdollPrefab = Main.StaticData.Game.Races.GetValue(unit.UnitData.RaceName).Graphics.RagdollObject;
        if (ragdollPrefab == null)
        {
            Debug.LogWarning("No ragdoll object for " + name);
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.useGravity = true;
            return;
        }

        GameObject ragdollObject = Instantiate(ragdollPrefab, transform);
        ragdoll = ragdollObject.GetComponent<UnitRagdoll>();
        Debug.Assert(ragdoll != null, "No UnitRagdoll found in ragdoll object");
        Destroy(unit.Graphics.RaceModel);
        Destroy(collider);
        Destroy(rigidbody);
        rigidbody = ragdoll.GetComponentInChildren<Rigidbody>();

        isRagdoll = true;
    }

    public void ApplyForce(Vector3 origin, float power)
    {
        Debug.Log($"Applying force of {power} to {gameObject.name}");
        if (isRagdoll)
        {
            Ragdoll.ApplyForce(origin, power);
        }
        else
        {
            ApplyForce(Rigidbody, origin, power);
        }
    }

    [Button]
    private void ApplyDefaultForce()
    {
        ApplyForce(Vector3.zero, defaultForce);
    }

    public static void ApplyForce(Rigidbody rigidbody, Vector3 origin, float power)
    {
        Vector3 directionVector = rigidbody.transform.position - origin;
        directionVector.Normalize();
        Vector3 forceVector = directionVector * power;
        Vector3 randomOffset = Random.insideUnitSphere / 2;
        rigidbody.AddForceAtPosition(forceVector, rigidbody.position + randomOffset, ForceMode.Impulse);
    }

    public Rigidbody Rigidbody => rigidbody;
    public Collider Collider => collider;
    public UnitRagdoll Ragdoll => ragdoll;
}
