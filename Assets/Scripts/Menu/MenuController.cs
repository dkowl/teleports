using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: try making this a ScriptableObject

//singleton controlling menu stack
public class MenuController : MonoBehaviour {

    private static MenuController instance;

    private Menu[] menus;
    private Stack<Menu> menuStack;

	public enum MenuType { CreateCharacter, ChooseCharacter, Home, Count };
    public Menu[] menuInspectorLinks;
    public MenuType startMenu;
    public string mainCanvasPrefabPath;


    //unity event functions
    void Awake()
    {
        instance = this;

        menus = new Menu[(int)MenuType.Count];
        menuStack = new Stack<Menu>();

        foreach(Menu menu in menuInspectorLinks)
        {
            menus[(int)menu.menuType] = menu;
        }

        OpenMenu(startMenu);
    }

    void OnDestroy()
    {
        CloseAll();
        instance = null;
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
        if(menu == menuStack.Peek())
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

    //protected functions
    protected void CloseAll()
    {
        while(menuStack.Count > 0)
        {
            CloseTopMenu();
        }
    }

    protected void HideAll()
    {
        foreach (Menu menu in menuStack)
        {
            menu.Hide();
        }
    }

    //will close all menus with disableMenusUnder set to false (close popups)
    protected void ShowTopMenus()
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
        get { return instance; }
    }

    public static string MainCanvasPrefabPath
    {
        get { return instance.mainCanvasPrefabPath; }
    }
}
