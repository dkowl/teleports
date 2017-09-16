using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "teleportData", menuName = "Custom/TeleportData", order = 1)]
public class TeleportData : ScriptableObject {

    [SerializeField]
    private float power;

    [SerializeField]
    [FormerlySerializedAs("time_")]
    private float time;


    public float Power
    {
        get { return power; }
    }

    public float Time
    {
        get { return time; }
    }

}
