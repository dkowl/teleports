using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Teleports.Utils;

public partial class TeleportData {

    private static readonly int GemSlotNo = 6;

    public TeleportData()
    {
        tier = 1;
        power = new Attribute(100);
        time = new Attribute(60);
        Utils.InitWithNew(ref gemSlots, GemSlotNo);
        graphicsId = "T_001";

        CorrectInvalidData();
    }

    public void CorrectInvalidData()
    {
        if(tier < 1)
        {
            tier = 1;
        }
        if(gemSlots == null || gemSlots.Count < GemSlotNo)
        {
            Debug.LogWarning("Gem slots not found, initializing...");
            Utils.InitWithNew(ref gemSlots, GemSlotNo);
        }
    }

    public GemSlot GetGemSlot(int id)
    {
        return gemSlots[id];
    }

    public TeleportGraphics Graphics => MainData.Game.GraphicsData.Teleport.TryGetValue(graphicsId);
}
