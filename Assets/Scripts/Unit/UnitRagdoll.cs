using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

public class UnitRagdoll : MonoBehaviour
{

    [SerializeField] private List<Rigidbody> mainRigidbodies;

    [Button]
    public void FindRigidbodies()
    {
        Rigidbody firstRb = GetComponentInChildren<Rigidbody>();
        mainRigidbodies = firstRb.GetComponentsInSiblings<Rigidbody>();
        mainRigidbodies.Add(firstRb);
    }

    public void ApplyForce(Transform origin, float power)
    {
        foreach (var rigidbody in mainRigidbodies)
        {
            UnitPhysics.ApplyForce(rigidbody, origin, power/mainRigidbodies.Count);
        }
    }
}