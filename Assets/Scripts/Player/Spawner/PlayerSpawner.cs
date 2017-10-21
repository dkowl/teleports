using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSpawner {

	public static GameObject Spawn(PlayerSpawnerParams p)
    {
        GameObject player;
        
        player = new GameObject("Player");
        player.transform.localPosition = Vector3.zero;
        player.transform.localEulerAngles = new Vector3(0, 180, 0);
        player.transform.parent = p.ParentObject.transform;
        player.tag = "Player";
        

        //Common
        IPlayerData playerData = MainData.CurrentPlayerData;
        RaceGraphics raceGraphics = MainData.CurrentGameData.GetRace(playerData.RaceName).Graphics;

        //Spawn model
        GameObject playerModel = Object.Instantiate(raceGraphics.ModelObject, player.transform);
        playerModel.transform.localEulerAngles = Vector3.zero;

        Animator animator = playerModel.GetComponentInChildren<Animator>();

        switch (p.Type)
        {
            case PlayerSpawnerParams.SpawnType.UI:

                foreach (ItemData itemData in playerData.InventoryData.GetEquippedItems())
                {
                    ItemSpawner.Spawn(p.ParentObject, itemData);
                }
                animator.runtimeAnimatorController = raceGraphics.UiAnimationController;
                break;               

            case PlayerSpawnerParams.SpawnType.World:

                //Components to be initialized
                Unit unit = player.GetComponent<Unit>();
                PlayerController controller = player.GetComponent<PlayerController>();
                XpComponent xp = player.GetComponent<XpComponent>();

                if (unit == null)
                {
                    unit = player.AddComponent<Unit>();
                }
                unit.unitData = playerData.BaseUnitData;

                if (controller == null)
                {
                    controller = player.AddComponent<PlayerController>();
                }

                if (xp == null)
                {
                    xp = player.AddComponent<XpComponent>();
                }
                xp.Xp = playerData.Xp;

                //Instantiating skills
                GameObject skills = new GameObject("Skills");
                skills.transform.parent = player.transform;

                GameObject primarySkill = Object.Instantiate(MainData.CurrentGameData.GetSkill(playerData.PrimarySkillId).gameObject, skills.transform);
                controller.MainAttack = primarySkill.GetComponent<Skill>();
                unit.ActiveController = controller;

                //Instantiating items

                GameObject items = new GameObject("Items");
                items.transform.parent = player.transform;

                foreach (ItemData itemData in playerData.InventoryData.GetEquippedItems())
                {
                    GameObject itemObject = new GameObject(itemData.UniqueName);
                    itemObject.transform.parent = items.transform;
                    Item item = itemObject.AddComponent<Item>();
                    item.Data = itemData;
                }

                //Set up Animator
                animator.runtimeAnimatorController = raceGraphics.WorldAnimationController;
                animator.gameObject.AddComponent<UnitAnimator>();

                player.AddComponent<PlayerWorldUI>();
                break;
        }

        return player;
        
    }
}
