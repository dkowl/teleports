using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndTeleportMS : ModelSpawner, IMessageHandler<ItemEquipMessage> {

    protected override GameObject GetModel(int id)
    {
        if (MainData.CurrentPlayerData == null) return null;
        if(id%2 == 0)
        {
            var result = UnitModelAssembler.GetModel(MainData.CurrentPlayerData.UnitData, true, true);
            result.transform.parent = transform;
            return result;
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
        MainData.MessageBus.Subscribe(this);
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        SaveData.OnCharacterIDChangedEvent -= ShouldRespawn;
        MainData.MessageBus.Unsubscribe(this);
    }

    public void Handle(ItemEquipMessage message)
    {
        ShouldRespawn();
    }
}
