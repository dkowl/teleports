using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class MenuSwitcherButtonUISpawner : PrefabSpawner {
    
    public Transform parentTransform;
    [OdinSerialize] public List<Tuple<MenuID, string>> menus;

    protected override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = SpawnedInstance.GetComponent<MenuSwitcherButtonUI>();
        Debug.Assert(menus != null && menus.Count > CurrentInstanceId);

        button.MenuId = menus[CurrentInstanceId].Item1;
        button.ButtonString = menus[CurrentInstanceId].Item2;
        SpawnedInstance.gameObject.transform.SetParent(parentTransform);

        button.LoadData();
    }
}
