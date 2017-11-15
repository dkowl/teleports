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
    [NonSerialized] private CommandQueue commandQ = new CommandQueue();

    //inspector variables
    [SerializeField] private GameObject prefab;
    [SerializeField] private MenuController.MenuType menuType;
    [SerializeField] private bool disableMenusUnder;
    [Tooltip("will parent the menu to the MainCanvas prefab")]
    [SerializeField] private bool useMainCanvas;

    //static events for LoadableBehaviours
    public delegate void OnShow();
    public static event OnShow OnShowEvent;

    public delegate void OnHide();
    public static event OnHide OnHideEvent;

    //non-static events for MenuCommands
    public delegate void CommandFinish();
    public event CommandFinish ShowFinishEvent, HideFinishEvent;

    //public functions
    public void AddCommand(MenuCommand.Type type)
    {
        if(type == MenuCommand.Type.Close)
        {
            AddCommand(MenuCommand.Type.Hide);
        }
        commandQ.AddCommand(new MenuCommand(this, type));
    }

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
            AddCommand(MenuCommand.Type.Show);
        }
    }

    public void Show()
    {
        if (IsOpen && !isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(true);

            Command[] commands = new MenuBehaviourCommand[menuBehaviours.Length];
            for(int i = 0; i<menuBehaviours.Length; i++)
            {
                commands[i] = new MenuBehaviourCommand(menuBehaviours[i], MenuBehaviourCommand.Type.Open, true);
            }
            MultiCommand multiCommand = new MultiCommand(commands);
            multiCommand.RegisterFinishCallback(ShowFinish);
            commandQ.AddCommand(multiCommand, CommandQueue.AddMode.Instant);
        }
        else
        {
            ShowFinish();
        }
    }

    public void ShowFinish()
    {
        isActive = true;

        if (OnShowEvent != null)
        {
            OnShowEvent();
        }
        if(ShowFinishEvent != null)
        {
            ShowFinishEvent();
        }
    }

    public void Close()
    {
        if (IsOpen && instantiatedObject != null)
        {            
            Destroy(instantiatedObject);
            instantiatedObject = null;
            IsOpen = false;            
        }
    }

    public void Hide()
    {
        if(IsOpen && isActive && instantiatedObject != null)
        {
            Command[] commands = new MenuBehaviourCommand[menuBehaviours.Length];
            for (int i = 0; i < menuBehaviours.Length; i++)
            {
                commands[i] = new MenuBehaviourCommand(menuBehaviours[i], MenuBehaviourCommand.Type.Close, true);
            }
            MultiCommand multiCommand = new MultiCommand(commands);
            multiCommand.RegisterFinishCallback(HideFinish);
            commandQ.AddCommand(multiCommand);
        }
        else
        {
            HideFinish();
        }
    }

    public void HideFinish()
    {
        instantiatedObject.SetActive(false);

        isActive = false;

        if (OnHideEvent != null)
        {
            OnHideEvent();
        }

        if(HideFinishEvent != null)
        {
            HideFinishEvent();
        }
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

    public GameObject InstantiatedObject
    {
        get { return instantiatedObject; }
    }
}
