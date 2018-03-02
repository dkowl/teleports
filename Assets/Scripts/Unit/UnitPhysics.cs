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
    private new CapsuleCollider collider;
    private UnitRagdoll ragdoll;
    private bool isRagdoll;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        SpawnRigidbody();
        SpawnCollider();
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

    private void SpawnCollider()
    {
        if (GetComponentInChildren<Collider>() == null)
        {
            collider = gameObject.AddComponent<CapsuleCollider>();
        }
        if (collider == null) return;
        collider.radius = unit.Size;
        collider.height = unit.UnitData.Height + unit.Size;
        collider.center = new Vector3(0, unit.UnitData.Height / 2, 0);
    }

    public void SwitchToRagdoll()
    {
        var ragdollObject = Instantiate(MainData.Game.GetRace(unit.UnitData.RaceName).Graphics.RagdollObject, transform);
        if (ragdollObject == null) return;
        
        ragdoll = ragdollObject.GetComponent<UnitRagdoll>();
        Debug.Assert(ragdoll != null, "No UnitRagdoll found in ragdoll object");
        Destroy(unit.Graphics.RaceModel);
        Destroy(collider);
        rigidbody = ragdoll.GetComponentInChildren<Rigidbody>();
        unit.gameObject.SetLayerIncludingChildren(0);

        isRagdoll = true;
    }

    public void ApplyForce(Transform origin, float power)
    {
        if (isRagdoll)
        {
            Ragdoll.ApplyForce(origin, power);
        }
        else
        {
            ApplyForce(Rigidbody, origin, power);
        }
    }

    public static void ApplyForce(Rigidbody rigidbody, Transform origin, float power)
    {
        Vector3 directionVector = rigidbody.transform.position - origin.position;
        directionVector.Normalize();
        Vector3 forceVector = directionVector * power;
        Vector3 randomOffset = Random.insideUnitSphere / 2;
        rigidbody.AddForceAtPosition(forceVector, rigidbody.position + randomOffset, ForceMode.Impulse);
    }

    public Rigidbody Rigidbody => rigidbody;
    public Collider Collider => collider;
    public UnitRagdoll Ragdoll => ragdoll;
}
