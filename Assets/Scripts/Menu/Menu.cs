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
    public delegate void OnOpen();
    public static event OnOpen OnOpenEvent;

    public delegate void OnClose();
    public static event OnClose OnCloseEvent;

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
            else
            {
                instantiatedObject.SetActive(true);
            }

            menuBehaviours = instantiatedObject.GetComponentsInChildren<MenuBehaviour>();
            foreach(var menuBehaviour in menuBehaviours)
            {
                menuBehaviour.OnOpen();
            }
            IsOpen = true;

            if (OnOpenEvent != null)
            {
                OnOpenEvent();
            }
        }
    }

    public void Close()
    {
        if (IsOpen && instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
            IsOpen = false;
            if (OnCloseEvent != null)
            {
                OnCloseEvent();
            }
        }
    }

    public void Show()
    {
        if (IsOpen && !isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(true);
            isActive = true;
        }
    }

    public void Hide()
    {
        if(IsOpen && isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(false);
            isActive = false;
        }
    }

    //properties
    public bool IsOpen
    {
        get { return isOpen; }
        private set
        {
            isOpen = value;
            isActive = value;
        }
    }
    public bool IsActive
    {
        get { return isActive; }
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
