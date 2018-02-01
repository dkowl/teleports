using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitModelAssembler {

	public static GameObject GetModel(UnitData unitData)
    {
        GameObject result;

        result = Object.Instantiate(MainData.Game.GetRace(unitData.RaceName).Graphics.ModelObject);

        foreach(var itemInfo in unitData.Inventory.EquipmentData.GetEquippedItems())
        {
            Debug.Log("Spawning " + itemInfo.Item.DisplayName);
            ItemSpawner.Spawn(result, itemInfo.Item, itemInfo.PrimarySlot);
        }

        return result;
    }
}
