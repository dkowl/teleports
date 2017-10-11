using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton controlling menu stack
[ExecuteInEditMode]
[CreateAssetMenu(fileName = "menuController", menuName = "Menu/Controller")]
public class MenuController : ScriptableObject {

    private static MenuController instance;


    [System.NonSerialized] private Menu[] menus = new Menu[(int)MenuType.Count];
    [System.NonSerialized] private Stack<Menu> menuStack;
    [System.NonSerialized] private Transform spawnTransform;

	public enum MenuType { CreateCharacter, ChooseCharacter, Popup, Count };
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

    public void FirstStart(Transform newSpawnTransform)
    {
        spawnTransform = newSpawnTransform;

        OpenMenu(startMenu);
    }

    protected void Initialize()
    {
        instance = this;
        menus = new Menu[(int)MenuType.Count];
        menuStack = new Stack<Menu>();

        foreach (Menu menu in menuInspectorLinks)
        {
            menus[(int)menu.menuType] = menu;
        }

        Debug.Log("Menu Controller initialized!");
    }

    //public functions
    public void OpenMenu(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu != null && !menu.IsOpen)
        {
            if (menu.disableMenusUnder)
            {
                HideAll();
            }
            menuStack.Push(menu);
            menu.Open();
        }
    }

    public void CloseMenu(MenuType menuType)
    {
        Menu menu = menus[(int)menuType];
        if (menu == menuStack.Peek())
        {
            CloseTopMenu();
            if (menu.disableMenusUnder)
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
            menu.Close();
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

    public void CloseAll()
    {
        while(menuStack.Count > 0)
        {
            CloseTopMenu();
        }
    }

    //protected functions
    public void HideAll()
    {
        foreach (Menu menu in menuStack)
        {
            menu.Hide();
        }
    }

    //will close all menus with disableMenusUnder set to false (close popups)
    public void ShowTopMenus()
    {
        while(menuStack.Count != 0)
        {
            Menu menu = menuStack.Peek();
            if (!menu.disableMenusUnder)
            {
                CloseTopMenu();
            }
            else
            {
                menu.Show();
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
