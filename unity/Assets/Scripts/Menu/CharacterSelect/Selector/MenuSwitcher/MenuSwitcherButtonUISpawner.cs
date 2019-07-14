using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class MenuSwitcherButtonUISpawner : PrefabSpawner {
    
    [SerializeField] private Transform parentTransform;
    [OdinSerialize] private List<Tuple<MenuID, string>> menus;

    private bool deactivatingBeforeActivatingFlag;
    private bool lastDeactivationWasCausedByActivation;

    public void DeactivateAll()
    {
        bool hadAnyActiveButtons = false;
        foreach (var button in SpawnedButtons)
        {
            if (button.IsActive)
            {
                button.Deactivate();
                hadAnyActiveButtons = true;
            }
        }

        lastDeactivationWasCausedByActivation = hadAnyActiveButtons && deactivatingBeforeActivatingFlag;
        deactivatingBeforeActivatingFlag = false;
    }

    protected override void AfterSpawn()
    {
        MenuSwitcherButtonUI button = SpawnedInstance.GetComponent<MenuSwitcherButtonUI>();
        Debug.Assert(menus != null && menus.Count > CurrentInstanceId);
        Debug.Assert(button != null);

        var menuId = menus[CurrentInstanceId].Item1;
        button.MenuId = menuId;
        button.UpdateNotification(menuId);
        button.ButtonString = menus[CurrentInstanceId].Item2;
        button.Spawner = this;

        SpawnedInstance.gameObject.transform.SetParent(parentTransform);
    }

    private void Update()
    {
        for(int i = 0; i < menus.Count; i++)
        {
            var menuId = menus[i].Item1;
            var button = spawnedInstances[i].GetComponent<MenuSwitcherButtonUI>();
            button.UpdateNotification(menuId);
        }
    }

    public bool DeactivatingBeforeActivatingFlag
    {
        set { deactivatingBeforeActivatingFlag = value; }
    }

    public bool LastDeactivationWasCausedByActivation => lastDeactivationWasCausedByActivation;

    private List<MenuSwitcherButtonUI> SpawnedButtons
    {
        get
        {
            var result = new List<MenuSwitcherButtonUI>();
            foreach (var spawnedInstance in spawnedInstances)
            {
                MenuSwitcherButtonUI button = spawnedInstance.GetComponent<MenuSwitcherButtonUI>();
                Debug.Assert(button != null);
                result.Add(button);
            }

            return result;
        }
    }

    public bool HasAnyActive
    {
        get
        {
            foreach(var button in SpawnedButtons)
            {
                if (button.IsActive) return true;
            }
            return false;
        }
    }
}
