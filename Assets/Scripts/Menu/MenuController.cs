using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//singleton controlling menu stack
[ExecuteInEditMode]
[CreateAssetMenu(fileName = "menuController", menuName = "Menu/Controller")]
public class MenuController : ScriptableObject
{
    public enum MenuType { CreateCharacter, ChooseCharacter, Popup, TestA, TestB, Count };

    private static MenuController instance;

    [NonSerialized] private Menu[] menus = new Menu[(int)MenuType.Count];
    [NonSerialized] private Stack<Menu> menuStack;
    [NonSerialized] private Transform spawnTransform;
    [NonSerialized] private CommandQueue commandQ = new CommandQueue();

    [SerializeField] private Menu[] menuInspectorLinks;
    [SerializeField] private MenuType startMenu;
    [SerializeField] private GameObject mainCanvasPrefab;


    //unity event functions
    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        Initialize();
    }

    void OnDestroy()
    {
        CloseAll();
        instance = null;
    }

    protected void Initialize()
    {
        instance = this;
        menus = new Menu[(int)MenuType.Count];
        menuStack = new Stack<Menu>();

        foreach (Menu menu in menuInspectorLinks)
        {
            menus[(int)menu.MenuType] = menu;
        }

        Debug.Log("Menu Controller initialized!");
    }

    protected void AddCommand(Menu menu, MenuCommand.Type type)
    {
        if (type == MenuCommand.Type.Close)
        {
            AddCommand(menu, MenuCommand.Type.Hide);
        }
        commandQ.AddCommand(new MenuCommand(menu, type));
    }

    //public functions
    public void FirstStart(Transform newSpawnTransform)
    {
        spawnTransform = newSpawnTransform;

        OpenMenu(startMenu);
    }

    public void OpenMenu(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu != null && !menu.IsOpen)
        {
            if (menu.DisableMenusUnder)
            {
                HideAll();
            }
            menuStack.Push(menu);
            AddCommand(menu, MenuCommand.Type.Open);
        }
        else{
            ShowMenu(menuType);
        }
    }

    public void ShowMenu(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu != null && menu.IsOpen)
        {
            while(menuStack.Peek().MenuType != menuType)
            {
                CloseTopMenu();
            }
            AddCommand(menuStack.Peek(), MenuCommand.Type.Show);
        }
    }

    public void CloseMenu(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu == menuStack.Peek())
        {
            CloseTopMenu();
            if (menu.DisableMenusUnder)
            {
                ShowTopMenus();
            }
        }
    }

    public void CloseTopMenu()
    {
        if (menuStack.Count != 0)
        {
            Menu menu = menuStack.Pop();
            AddCommand(menu, MenuCommand.Type.Close);
        }
    }

    public void CloseAll()
    {
        while (menuStack.Count > 0)
        {
            CloseTopMenu();
        }
    }

    public void HideAll()
    {
        foreach (Menu menu in menuStack)
        {
            AddCommand(menu, MenuCommand.Type.Hide);
        }
    }

    public bool IsActive(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu != null)
        {
            return menus[(int)menuType].IsActive;
        }
        else
        {
            return false;
        }
    }    

    //will close all menus with disableMenusUnder set to false (close popups)
    public void ShowTopMenus()
    {
        while(menuStack.Count != 0)
        {
            Menu menu = menuStack.Peek();
            if (!menu.DisableMenusUnder)
            {
                CloseTopMenu();
            }
            else
            {
                AddCommand(menu, MenuCommand.Type.Show);
                break;
            }
        }
    }

    //properties
    public static MenuController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load("Menu/menuController") as MenuController;
            }
            return instance;
        }
    }

    public static Transform SpawnTransform
    {
        get
        {
            return Instance.spawnTransform;
        }
    }

    public static GameObject MainCanvasPrefab
    {
        get { return Instance.mainCanvasPrefab; }
    }
}
