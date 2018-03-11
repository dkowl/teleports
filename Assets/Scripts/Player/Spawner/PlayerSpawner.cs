using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public static class PlayerSpawner {

	public static GameObject Spawn(GameObject parentObject)
    {     
        //Common
        PlayerData playerData = MainData.CurrentPlayerData;
        if (playerData == null) return null;

        var playerObject = new GameObject("Player");
        playerObject.transform.localPosition = Vector3.zero;
        playerObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        playerObject.transform.parent = parentObject.transform;
        playerObject.tag = "Player";
        playerObject.layer = 9;          

        UnitData unitData = new UnitData(playerData.UnitData);
        unitData.Inventory.Add(MainData.Game.GetItem("Dagger"));
        unitData.Inventory.Equip("Dagger");
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
        xp.Xp = playerData.Xp;

        playerObject.AddComponent<PlayerWorldUI>();

        return playerObject;
        
    }
}
