using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndTeleportMS : ModelSpawner {

    protected override GameObject GetModel(int id)
    {
        if(id%2 == 0)
        {
            return UnitModelAssembler.GetModel(MainData.CurrentPlayerData.UnitData, true, true);
        }
        else
        {
            if (MainData.CurrentPlayerData != null)
            {
                return Instantiate(MainData.CurrentPlayerData.TeleportData.Graphics.modelObject, transform);
            }
            else
                return null;
        }
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        SaveData.OnCharacterIDChangedEvent += ShouldRespawn;
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        SaveData.OnCharacterIDChangedEvent -= ShouldRespawn;
    }
}
