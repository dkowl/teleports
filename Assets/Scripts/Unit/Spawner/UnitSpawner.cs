using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnitSpawner  {

	public static void SpawnItems(this Unit unit)
    {
        GameObject items = new GameObject("Items");
        items.transform.parent = unit.transform;

        foreach (var itemInfo in unit.UnitData.Inventory.GetEquippedItems())
        {
            Debug.Log("Spawning " + itemInfo.Item.DisplayName);
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
            var skillData = skillAsset.Data;
            GameObject skillGameObject = new GameObject(skillData.UniqueName);
            skillGameObject.transform.parent = skillsObject.transform;
            Skill skillComponent;
            switch (skillData.SkillType)
            {
                case SkillType.Attack:
                    skillComponent = skillGameObject.AddComponent<Attack>();
                    break;
                default:
                    skillComponent = null;
                    break;
            }

            if (skillComponent != null)
            {
                skillComponent.Data = skillData;
                unit.Skills.Add(skillComponent);
            }
        }
    }
}
