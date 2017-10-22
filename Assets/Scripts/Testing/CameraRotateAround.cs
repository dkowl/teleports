using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour {

    public Transform target;
    Camera cam;

    public float speed = 60;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
    }
}
