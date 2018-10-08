using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndTeleportMS : ModelSpawner, IMessageHandler<ItemEquipMessage> {

    protected override GameObject GetModel(int id)
    {
        HeroData heroData = Main.GameState.CurrentHeroData;

        if (heroData == null) return null;

        if(id%2 == 0)
        {
            var result = UnitModelAssembler.GetModel(heroData.UnitData, true, true);
            result.transform.parent = transform;
            return result;
        }
        else
        {
            return Instantiate(heroData.TeleportData.Graphics.modelObject, transform);
        }
    }

    protected override void SubscribeInternal()
    {
        base.SubscribeInternal();
        GameState.HeroChangedEvent += ShouldRespawn;
        Main.MessageBus.Subscribe(this);
    }

    protected override void UnsubscribeInternal()
    {
        base.UnsubscribeInternal();
        GameState.HeroChangedEvent -= ShouldRespawn;
        Main.MessageBus.Unsubscribe(this);
    }

    public void Handle(ItemEquipMessage message)
    {
        ShouldRespawn();
    }
}
