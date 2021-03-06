﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitModelAssembler {

	public static GameObject GetModel(UnitData unitData, bool withItems = true, bool withUiAnimationController = false)
    {
        var result = Object.Instantiate(Main.StaticData.Game.Races.GetValue(unitData.RaceName).Graphics.ModelObject);

        if (withItems)
        {
            foreach (var itemInfo in unitData.Inventory.EquipmentData.GetEquippedItems())
            {
                ItemSpawner.Spawn(result, itemInfo.Item, itemInfo.PrimarySlot);
            }
        }
        if(withUiAnimationController) AddUiAnimationController(result, unitData);

        return result;
    }

    public static void AddUiAnimationController(GameObject modelObject, UnitData unitData)
    {
        Animator animator = modelObject.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator not found");
            return;
        }
        RaceGraphics raceGraphics = Main.StaticData.Game.Races.GetValue(unitData.RaceName).Graphics;
        if (raceGraphics.UiAnimationController == null)
        {
            Debug.LogWarning("UiAnimationController not found");
            return;
        }
        animator.runtimeAnimatorController = Object.Instantiate(raceGraphics.UiAnimationController);
    }
}
