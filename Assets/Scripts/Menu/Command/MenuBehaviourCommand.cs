using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviourCommand : Command {

	public enum Type
    {
        Open,
        Close,
        Load
    }

    protected MenuBehaviour menuBehaviour;
    protected Type type;

    public MenuBehaviourCommand(MenuBehaviour menuBehaviour, Type type)
    {
        this.menuBehaviour = menuBehaviour;
        this.type = type;

        Debug.Log("Creating command " + type.ToString());
    }

    protected override void StartInternal()
    {
        switch (type)
        {
            case Type.Open:
                menuBehaviour.OpenFinishEvent += Finish;
                menuBehaviour.OnOpen();
                break;
            case Type.Close:
                menuBehaviour.CloseFinishEvent += Finish;
                menuBehaviour.OnClose();
                break;
            case Type.Load:
                menuBehaviour.LoadFinishEvent += Finish;
                menuBehaviour.OnLoad();
                break;
        }
    }

    protected override void FinishInternal()
    {
        switch (type)
        {
            case Type.Open:
                menuBehaviour.OpenFinishEvent -= Finish;
                break;
            case Type.Close:
                menuBehaviour.CloseFinishEvent -= Finish;
                break;
            case Type.Load:
                menuBehaviour.LoadFinishEvent -= Finish;
                break;
        }
    }
}
