using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class TeleportData {

    private static readonly int GemSlotNo = 6;

    [SerializeField]
    private int tier = 1;

    [SerializeField]
    private float power;

    [SerializeField]
    [FormerlySerializedAs("time_")]
    private float time;

    [SerializeField]
    private GemSlot[] gemSlots = new GemSlot[GemSlotNo];

    [SerializeField]
    private string graphicsId;

    public int Tier
    {
        get { return tier; }
    }


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

    public TeleportGraphics Graphics
    {
        get { return MainData.CurrentGameData.GraphicsData.Teleport.TryGetValue(graphicsId); }
    }

}
