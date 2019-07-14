using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class MenuSwitcherButtonUI : MonoBehaviour {

    [SerializeField] private float inactiveScaleFactor = 0.9f;
    [SerializeField] private Text buttonText;
    [SerializeField] private GameObject notification;
    [SerializeField] private Text notificationText;

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
        if (!spawner.HasAnyActive) MenuController.Instance.CloseMenu(MenuController.MenuIdHomeForeground);
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

    public void UpdateNotification(MenuID menuId)
    {
        if (menuId == "Attributes" && Main.GameState.CurrentHeroData.TotalAttributePoints != 0)
        {
            ShowNotification = true;
            NotificationText = Main.GameState.CurrentHeroData.TotalAttributePoints.ToString();
        }
        else
        {
            ShowNotification = false;
        }
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

    private bool ShowNotification { set { notification.SetActive(value); } }
    private string NotificationText { set { notificationText.text = value; } }
}
