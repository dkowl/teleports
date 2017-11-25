using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour {

    public Transform target;

    public float speed = 60;

    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
    }
}
