using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcherButtonUISpawner : PrefabSpawner {

    public MenuController.MenuType menuType;
    public string buttonString;
    public Transform parentTransform;

    public override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = spawnedInstance.GetComponent<MenuSwitcherButtonUI>();

        button.MenuType = menuType;
        button.ButtonString = buttonString;
        spawnedInstance.gameObject.transform.SetParent(parentTransform);

        button.LoadData();
    }
}
