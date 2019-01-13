using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerStub : IMenuController {

    public void FirstStart() { }
    public void OpenMenu(MenuID menuId) { }
    public void ShowMenu(MenuID menuId) { }
    public void CloseMenu(MenuID menuId) { }
    public void CloseTopMenu() { }
    public void CloseAll() { }
    public void HideAll() { }

    public bool IsActive(MenuID menuId)
    {
        return false;
    }

    public Menu GetMenu(MenuID menuId)
    {
        return null;
    }
}
