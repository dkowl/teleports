using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcherButtonUISpawner : PrefabSpawner {

    public MenuController.MenuType menuType;
    public string buttonString;
    public Transform parentTransform;

    protected override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = SpawnedInstance.GetComponent<MenuSwitcherButtonUI>();

        button.MenuType = menuType;
        button.ButtonString = buttonString;
        SpawnedInstance.gameObject.transform.SetParent(parentTransform);

        button.LoadData();
    }
}
