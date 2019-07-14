using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestructionEffect : MonoBehaviour
{

    enum DestructionEffect
    {
        None,
        Shrink
    }

    float timeLeft;
    bool isStarted;

    [SerializeField]
    float delayTime;

    void Update()
    {
        if (isStarted)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (delayTime > 0)
            {
                timeLeft = delayTime;
                isStarted = true;
            }
        }
    }

    public float DelayTime
    {
        set { delayTime = value; }
    }

}
