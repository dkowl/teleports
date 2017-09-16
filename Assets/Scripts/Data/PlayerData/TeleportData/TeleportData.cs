using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "teleportData", menuName = "Custom/TeleportData", order = 1)]
public class TeleportData : ScriptableObject {

    private static readonly int GemSlotNo = 6;

    [SerializeField]
    private float power;

    [SerializeField]
    [FormerlySerializedAs("time_")]
    private float time;

    [SerializeField]
    private GemSlot[] gemSlots = new GemSlot[GemSlotNo];


    public float Power
    {
        get { return power; }
    }

    public float Time
    {
        get { return time; }
    }

    public GemSlot GetGemSlot(int id)
    {
        return gemSlots[id];
    }

}
