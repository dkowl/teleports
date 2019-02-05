using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class MenuSwitcherButtonUISpawner : PrefabSpawner {

    public MenuID menuId;
    public string buttonString;
    public Transform parentTransform;
    [OdinSerialize] public List<Tuple<MenuID, string>> menus;

    public void Rework()
    {
        menus = new List<Tuple<MenuID, string>> {new Tuple<MenuID, string>(menuId, buttonString)};
    }

    protected override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = SpawnedInstance.GetComponent<MenuSwitcherButtonUI>();

        button.MenuId = menuId;
        button.ButtonString = buttonString;
        SpawnedInstance.gameObject.transform.SetParent(parentTransform);

        button.LoadData();
    }
}
