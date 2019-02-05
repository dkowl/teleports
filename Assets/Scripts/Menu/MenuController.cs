using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuController : IMenuController
{
    public static readonly MenuID MenuIdHome = new MenuID("Home");
    public static readonly MenuID MenuIdSelectHero = new MenuID("SelectHero");
    public static readonly MenuID MenuIdCreateHero = new MenuID("CreateHero");
    public static readonly MenuID MenuIdDialogWindow = new MenuID("DialogWindow");

    [NonSerialized] private Dictionary<MenuID, Menu> menus;
    [NonSerialized] private Stack<Menu> menuStack;
    [NonSerialized] private CommandQueue commandQ = new CommandQueue();

    public MenuController()
    {
        menus = new Dictionary<MenuID, Menu>();
        menuStack = new Stack<Menu>();

        foreach (MenuData menuData in Main.StaticData.UI.Menus.AllValues)
        {
            var menuId = new MenuID(menuData.UniqueName);
            menus.Add(menuId, new Menu(menuId));
        }

        Debug.Log("Menu Controller initialized!");
    }
    
    public void FirstStart()
    {
        if (Main.GameState.CurrentHeroData != null)
            OpenMenu(MenuIdHome);
        else
            OpenMenu(MenuIdSelectHero);
    }

    public void OpenMenu(MenuID menuId)
    {
        Menu menu = GetMenu(menuId);
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
            ShowMenu(menuId);
        }
    }

    public void ShowMenu(MenuID menuId)
    {
        Menu menu = GetMenu(menuId);
        if (menu == null || !menu.IsOpen) return;
        while(!menuStack.Peek().MenuId.Equals(menuId))
        {
            CloseTopMenu();
        }
        AddCommand(menuStack.Peek(), MenuCommand.Type.Show);
    }

    public void CloseMenu(MenuID menuId)
    {
        Menu menu = GetMenu(menuId);
        if (menu != menuStack.Peek()) return;
        CloseTopMenu();
        if (menu.DisableMenusUnder)
        {
            ShowTopMenus();
        }
    }

    public void CloseTopMenu()
    {
        if (menuStack.Count == 0) return;
        Menu menu = menuStack.Pop();
        AddCommand(menu, MenuCommand.Type.Close);
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

    public bool IsActive(MenuID menuId)
    {
        if (!menus.ContainsKey(menuId)) return false;
        Menu menu = GetMenu(menuId);
        return menu != null && menu.IsActive;
    }

    public Menu GetMenu(MenuID menuId)
    {
        if (!menus.ContainsKey(menuId))
        {
            Debug.LogWarning($"Asking for non-existing menu {menuId}");
            return null;
        }
        return menus[menuId];
    }

    //will close all menus with disableMenusUnder set to false (close popups)
    private void ShowTopMenus()
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

    public void DisplayMenuStack()
    {
        Debug.Log("Stack Begin v");
        foreach(var menu in menuStack)
        {
            Debug.Log(menu.MenuId);
        }
        Debug.Log("Stack End ^");
    }

    private void AddCommand(Menu menu, MenuCommand.Type type)
    {
        if (type == MenuCommand.Type.Close)
        {
            AddCommand(menu, MenuCommand.Type.Hide);
        }
        commandQ.AddCommand(new MenuCommand(menu, type));
    }

    // deprecated
    public static IMenuController Instance => Main.UISystem.MenuController;
}
