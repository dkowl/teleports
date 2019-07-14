using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuSpawner : PrefabSpawner
{
    [SerializeField] private MenuID menuId;
    [SerializeField] private GameObject content;

    protected override void PreInitialize()
    {
        prefab = Main.StaticData.UI.PopupMenuPrefab;
        Debug.Assert(prefab != null);
        spawnAmount = 1;
    }

    protected override void AfterSpawn()
    {
        var popupMenu = SpawnedInstance.GetComponent<PopupMenuUI>();
        Debug.Assert(popupMenu != null);

        popupMenu.Init(menuId, content);
    }
}
