using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float period_;
    public Vector3 rotation_;

    float value_;
    Vector3 startRotation_;

    
	void Start () {
        startRotation_ = transform.rotation.eulerAngles;
	}
	
	void Update () {
        value_ += Time.deltaTime / period_;

        if(value_ > 1)
        {
            value_--;
        }

	}

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(startRotation_ + rotation_ * value_);
    }
}
