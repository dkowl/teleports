using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnitSpawner  {

    public static Unit SpawnUnit(GameObject gameObject, UnitData unitData)
    {
        GameObject raceObject = UnitModelAssembler.GetModel(unitData);
        raceObject.transform.parent = gameObject.transform;
        raceObject.transform.localPosition = Vector3.zero;

        Unit unit = gameObject.AddComponent<Unit>();
        unit.UnitData = new UnitData(unitData);
        unit.Graphics.RaceModel = raceObject;

        unit.SpawnItems();
        unit.SpawnSkills();
        unit.SpawnAnimator();

        return unit;
    }

	public static void SpawnItems(this Unit unit)
    {
        GameObject items = new GameObject("Items");
        items.transform.parent = unit.transform;

        foreach (var itemInfo in unit.UnitData.Inventory.GetEquippedItems())
        {
            //Debug.Log("Spawning " + itemInfo.Item.DisplayName);
            ItemData itemData = itemInfo.Item;
            GameObject itemObject = new GameObject(itemData.DisplayName);
            itemObject.transform.parent = items.transform;
            Item item = itemObject.AddComponent<Item>();
            item.Data = itemData;
            item.PrimarySlot = itemInfo.PrimarySlot;
        }
    }

    public static void SpawnAnimator(this Unit unit)
    {
        RaceGraphics raceGraphics = MainData.Game.GetRace(unit.UnitData.RaceName).Graphics;
        Animator animator = unit.Graphics.RaceModel.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = Object.Instantiate(raceGraphics.WorldAnimationController);
            animator.gameObject.AddComponent<UnitAnimator>();
            if (unit.UnitData.UsesRootMotion)
            {
                animator.applyRootMotion = true;
                animator.gameObject.AddComponent<RootMotionRedirector>();
            }
            else
            {
                animator.applyRootMotion = false;
            }
        }
        else
        {
            Debug.LogWarning("No animator found");
        }
    }

    public static void SpawnSkills(this Unit unit)
    {
        GameObject skillsObject = new GameObject("Skills");
        skillsObject.transform.parent = unit.transform;

        List<SkillAssetData> skills = MainData.Game.Skills.GetValues(unit.UnitData.SkillIds);
        if (skills == null || skills.Count == 0)
        {
            Debug.LogWarning("Unit has no skills, adding default skill");
            skills.Add(MainData.Game.Skills.GetValue(DataDefaults.skillName));
        }

        foreach (var skillAsset in skills)
        {
            unit.Skills.Add(SkillSpawner.SpawnSkill(skillsObject, skillAsset.Data));
        }
    }
}
