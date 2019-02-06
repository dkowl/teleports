using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class MenuSwitcherButtonUI : MonoBehaviour {

    [SerializeField] private float inactiveScaleFactor = 0.9f;
    [SerializeField] private Text buttonText;

    private MenuID menuId;
    private MenuSwitcherButtonUISpawner spawner;

    private bool isActive;

    public void Activate()
    {
        if (isActive) return;
        Debug.Assert(spawner != null);
        spawner.DeactivatingBeforeActivatingFlag = true;
        spawner.DeactivateAll();
        transform.localScale = Vector3.one;
        MenuController.Instance.OpenMenu(menuId);
        isActive = true;
    }

    public void Deactivate()
    {
        if (!isActive) return;
        transform.localScale = Vector3.one * inactiveScaleFactor;
        MenuController.Instance.CloseMenu(menuId);
        isActive = false;
    }

    public MenuID MenuId
    {
        set { menuId = value; }
    }

    public string ButtonString
    {
        set
        {
            Debug.Assert(buttonText != null);
            buttonText.text = value;
        }
    }

    public MenuSwitcherButtonUISpawner Spawner
    {
        set { spawner = value; }
    }

    public bool IsActive => isActive;
}
