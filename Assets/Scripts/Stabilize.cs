using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilize : MonoBehaviour {

    public float strength_ = 1f, rotationStrength_ = 10f;

    void LateUpdate()
    {
        gameObject.transform.localPosition = shrink(gameObject.transform.localPosition, strength_ * Time.deltaTime);
        gameObject.transform.localRotation = Quaternion.Euler(shrink(gameObject.transform.localRotation.eulerAngles, rotationStrength_ * Time.deltaTime, true));
        print(gameObject.transform.localEulerAngles.y);
    }

    //shrinks x by y
    float shrink(float x, float y, bool isAngle = false)
    {
        float delta = 0;
        if (isAngle)
        {
            while (x > 180)
            {
                x -= 360;
                delta -= 360;
            }
        }

        if(x > 0)
        {
            x = Mathf.Max(x - y, 0);
        }
        else
        {
            x = Mathf.Min(x + y, 0);
        }

        if (isAngle)
        {
            x += delta;
        }
        return x;
    }

    Vector3 shrink(Vector3 x, float y, bool isAngle = false)
    {
        x.x = shrink(x.x, y, isAngle);
        x.y = shrink(x.y, y, isAngle);
        x.z = shrink(x.z, y, isAngle);
        return x;
    }
}
