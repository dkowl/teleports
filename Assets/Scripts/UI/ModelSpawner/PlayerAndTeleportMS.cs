using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndTeleportMS : ModelSpawner {

    protected override GameObject GetModel(int id)
    {
        if(id%2 == 0)
        {
            return PlayerSpawner.Spawn(new PlayerSpawnerParams(gameObject, PlayerSpawnerParams.SpawnType.UI));
        }
        else
        {
            if (MainData.CurrentPlayerData != null)
            {
                return Instantiate(MainData.CurrentPlayerData.CurrentTeleportData.Graphics.modelObject, transform);
            }
            else
                return null;
        }
    }

    protected override void SubscribeInternal()
    {
        SaveData.OnCharacterIDChangedEvent += ShouldRespawn;
    }

    protected override void UnsubscribeInternal()
    {
        SaveData.OnCharacterIDChangedEvent -= ShouldRespawn;
    }
}
