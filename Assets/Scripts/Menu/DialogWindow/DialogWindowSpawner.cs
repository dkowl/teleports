using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogWindowSpawner {

	public static void Spawn(string text, List<ButtonChoice> choices)
    {
        MenuController.Instance.OpenMenu(MenuController.MenuType.DialogWindow);
        Menu windowMenu = MenuController.Instance.GetMenu(MenuController.MenuType.DialogWindow);
        if (windowMenu.InstantiatedObject != null)
        {
            DialogWindow window = windowMenu.InstantiatedObject.GetComponent<DialogWindow>();
            if (window == null) window = windowMenu.InstantiatedObject.GetComponentInChildren<DialogWindow>();

            window.Setup(text, choices);
        }
    }
}
