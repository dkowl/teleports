using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Menu", menuName = "Menu/Menu")]
public class Menu : ScriptableObject
{
    //private variables
    [NonSerialized] private GameObject instantiatedObject = null;
    [NonSerialized] private bool isOpen = false, isActive = false;
    [NonSerialized] private MenuBehaviour[] menuBehaviours;

    //inspector variables
    [SerializeField] private GameObject prefab;
    [SerializeField] private MenuController.MenuType menuType;
    [SerializeField] private bool disableMenusUnder;
    [Tooltip("will parent the menu to the MainCanvas prefab")]
    [SerializeField] private bool useMainCanvas;

    //events
    public delegate void OnShow();
    public static event OnShow OnShowEvent;

    public delegate void OnHide();
    public static event OnHide OnHideEvent;

    //public functions
    public void Open()
    {
        if (!IsOpen)
        {
            if (instantiatedObject == null)
            {
                if (useMainCanvas)
                {
                    instantiatedObject = Instantiate(MenuController.MainCanvasPrefab, MenuController.SpawnTransform) as GameObject;
                    Instantiate(prefab, instantiatedObject.transform);
                }
                else
                {
                    instantiatedObject = Instantiate(prefab, MenuController.SpawnTransform) as GameObject;
                }
            }

            menuBehaviours = instantiatedObject.GetComponentsInChildren<MenuBehaviour>();
            IsOpen = true;
            Show();
        }
    }

    public void Show()
    {
        if (IsOpen && !isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(true);

            foreach (var menuBehaviour in menuBehaviours)
            {
                menuBehaviour.OnOpen();
            }

            isActive = true;

            if (OnShowEvent != null)
            {
                OnShowEvent();
            }
        }
    }

    public void Close()
    {
        if (IsOpen && instantiatedObject != null)
        {
            if (Hide())
            {
                Destroy(instantiatedObject);
                instantiatedObject = null;
                IsOpen = false;
            }
        }
    }

    public bool Hide()
    {
        if(IsOpen && isActive && instantiatedObject != null)
        {
            foreach (var menuBehaviour in menuBehaviours)
            {
                menuBehaviour.OnClose();
            }

            if (IsClosing) return false;

            instantiatedObject.SetActive(false);            

            isActive = false;

            if (OnHideEvent != null)
            {
                OnHideEvent();
            }
        }
        return true;
    }

    //properties
    public bool IsOpen
    {
        get { return isOpen; }
        private set
        {
            isOpen = value;
        }
    }
    public bool IsActive
    {
        get { return isActive; }
    }

    public bool IsClosing
    {
        get
        {
            foreach(var menuBehaviour in menuBehaviours)
            {
                if(menuBehaviour.CurrentState == MenuBehaviour.State.Closing)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public MenuController.MenuType MenuType
    {
        get { return menuType; }
    }

    public bool DisableMenusUnder
    {
        get { return disableMenusUnder; }
    }

    public bool UseMainCanvas
    {
        get { return useMainCanvas; }
    }
}
