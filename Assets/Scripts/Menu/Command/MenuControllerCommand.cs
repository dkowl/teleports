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

    protected Menu menu;
    protected Type type;

    public MenuControllerCommand(Menu menu, Type type)
    {
        this.menu = menu;
        this.type = type;
    }

    protected override void StartInternal()
    {
        switch (type)
        {
            case Type.Open:
                menu.Open();
                break;
            case Type.Show:
                menu.Show();
                break;
            case Type.Hide:
                menu.Hide();
                break;
            case Type.Close:
                menu.Close();
                break;

        }
    }

    protected override void FinishInternal()
    {
    }
}
