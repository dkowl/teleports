﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class TeleportData {

    private static readonly int GemSlotNo = 6;

    [SerializeField] private int tier = 1;
    [SerializeField] private float power;
    [FormerlySerializedAs("time_")]
    [SerializeField] private float time;
    [SerializeField] private GemSlot[] gemSlots = new GemSlot[GemSlotNo];
    [SerializeField] private string graphicsId;

    public TeleportData()
    {
        tier = 1;
        power = 100;
        time = 60;
        graphicsId = "T_001";
        CorrectInvalidData();
    }

    public void CorrectInvalidData()
    {
        if(tier < 1)
        {
            tier = 1;
        }
        if(gemSlots.Length < GemSlotNo)
        {
            gemSlots = new GemSlot[GemSlotNo];
        }
    }

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
