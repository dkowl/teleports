using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuController
{

    void FirstStart();
    void OpenMenu(MenuID menuId);
    void ShowMenu(MenuID menuId);
    void CloseMenu(MenuID menuId);
    void CloseTopMenu();
    void CloseAll();
    void HideAll();
    bool IsActive(MenuID menuId);
    Menu GetMenu(MenuID menuId);

}
