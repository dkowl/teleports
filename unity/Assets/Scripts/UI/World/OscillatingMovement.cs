using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingMovement : MonoBehaviour {

    public float period_ = 1;
    public Vector3 begin, amplitude;

    Vector3 startPos_;
    float value_ = 0;
    bool increasing_ = true;
    
    void Start()
    {
        startPos_ = transform.localPosition;
    }


	void Update () {

        float valueDelta = Time.deltaTime / period_;

        if (increasing_)
        {
            value_ += valueDelta;
        }
        else
        {
            value_ -= valueDelta;
        }

        if (value_ >= 1 && increasing_ || value_ <= 0 && !increasing_)
        {
            increasing_ = !increasing_;
        }

        transform.localPosition = startPos_ + begin + value_ * amplitude;
	}
}
