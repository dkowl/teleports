using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCommand : Command {

    public enum Type
    {
        Open,
        Show,
        Hide,
        Close
    }

    protected Menu menu;
    protected Type type;

    public MenuCommand(Menu menu, Type type)
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
                Finish();
                break;
            case Type.Show:
                menu.ShowFinishEvent += Finish;
                menu.Show();
                break;
            case Type.Hide:
                menu.HideFinishEvent += Finish;
                menu.Hide();
                break;
            case Type.Close:
                menu.Close();
                Finish();
                break;

        }
    }

    protected override void FinishInternal()
    {
        switch (type)
        {
            case Type.Show:
                menu.ShowFinishEvent -= Finish;
                break;
            case Type.Hide:
                menu.HideFinishEvent -= Finish;
                break;
        }
    }

}
