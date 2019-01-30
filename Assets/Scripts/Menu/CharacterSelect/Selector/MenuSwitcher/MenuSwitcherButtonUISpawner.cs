using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcherButtonUISpawner : PrefabSpawner {

    public MenuID menuId;
    public string buttonString;
    public Transform parentTransform;

    protected override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = SpawnedInstance.GetComponent<MenuSwitcherButtonUI>();

        button.MenuId = menuId;
        button.ButtonString = buttonString;
        SpawnedInstance.gameObject.transform.SetParent(parentTransform);

        button.LoadData();
    }
}
