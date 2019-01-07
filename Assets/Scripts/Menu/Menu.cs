using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu
{
    private GameObject instantiatedObject;
    private bool isOpen; 
    private bool isActive;
    private MenuBehaviour[] menuBehaviours;
    private CommandQueue commandQ = new CommandQueue();
    private readonly MenuData data;
    private readonly MenuID id;
    
    //static events for LoadableBehaviours
    public delegate void OnShow();
    public static event OnShow OnShowEvent;

    public delegate void OnHide();
    public static event OnHide OnHideEvent;

    //non-static events for MenuCommands
    public delegate void CommandFinish();
    public event CommandFinish ShowFinishEvent, HideFinishEvent;

    //public functions
    public Menu(MenuID id)
    {
        this.id = id;
        this.data = Main.StaticData.UI.Menus.GetValue(id);
    }

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
        if (IsOpen) return;
        if (instantiatedObject == null)
        {
            if (UseMainCanvas)
            {
                instantiatedObject = Main.UISystem.SpawnCanvas(id);
                Object.Instantiate(Prefab, instantiatedObject.transform);
            }
            else
            {
                instantiatedObject = Main.UISystem.SpawnPrefab(Prefab);
            }
        }

        menuBehaviours = instantiatedObject.GetComponentsInChildren<MenuBehaviour>();
        IsOpen = true;
        AddCommand(MenuCommand.Type.Show);
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

        OnShowEvent?.Invoke();
        ShowFinishEvent?.Invoke();
    }

    public void Close()
    {
        if (IsOpen && instantiatedObject != null)
        {            
            Object.Destroy(instantiatedObject);
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

        OnHideEvent?.Invoke();
        HideFinishEvent?.Invoke();
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
    public bool IsActive => isActive;

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
    
    public bool DisableMenusUnder => data.DisableMenusUnder;
    public bool UseMainCanvas => data.UseMainCanvas;
    public GameObject InstantiatedObject => instantiatedObject;
    public MenuID MenuId => id;

    private GameObject Prefab => data.Prefab;
}
