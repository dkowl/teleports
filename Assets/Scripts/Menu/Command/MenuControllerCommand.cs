using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerCommand : Command {

    public enum Type
    {
        Open,
        Show,
        Hide,
        Close
    }

    protected MenuController.MenuType menu;
    protected Type type;

    public MenuControllerCommand(MenuController.MenuType menu, Type type)
    {
        this.menu = menu;
        this.type = type;
    }

    protected override void StartInternal()
    {
        switch (type)
        {
            case Type.Open:
                break;
            case Type.Show:
                break;
            case Type.Hide:
                break;
            case Type.Close:
                break;

        }
    }

    protected override void FinishInternal()
    {
    }
}
