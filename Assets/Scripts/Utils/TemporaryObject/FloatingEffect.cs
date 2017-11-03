using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour {

    [SerializeField] Vector3 gravity;
    [SerializeField] Vector3 startVelocity;
    Vector3 velocity;

    void Start()
    {
        velocity = startVelocity;
    }

    void Update()
    {
        float dTime = Time.deltaTime;

        velocity += gravity * dTime;
        transform.localPosition += velocity;
    }

    public Vector3 Gravity
    {
        set { gravity = value; }
    }

    public Vector3 StartVelocity
    {
        set { startVelocity = value; }
    }
}
