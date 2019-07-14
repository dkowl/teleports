using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSpawner {

	public static GameObject Spawn(GameObject parentObject)
    {     
        //Common
        HeroData heroData = Main.GameState.CurrentHeroData;
        if (heroData == null) return null;

        var playerObject = new GameObject("Player");
        playerObject.transform.localPosition = Vector3.zero;
        playerObject.transform.parent = parentObject.transform;
        playerObject.tag = "Player";
        playerObject.layer = 9;          

        UnitData unitData = new UnitData(heroData.UnitData);
        //unitData.Inventory.Add(MainData.Game.GetItem("Dagger"));
        //unitData.Inventory.Equip("Dagger");
        Unit unit = UnitSpawner.SpawnUnit(playerObject, unitData);

        //Components to be initialized
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        XpComponent xp = playerObject.GetComponent<XpComponent>();

        if (controller == null)
        {
            controller = playerObject.AddComponent<PlayerController>();
        }
        controller.MainAttack = unit.Skills[0];
        unit.ActiveController = controller;

        if (xp == null)
        {
            xp = playerObject.AddComponent<XpComponent>();
        }

        playerObject.AddComponent<PlayerWorldUI>();

        return playerObject;
        
    }
}
