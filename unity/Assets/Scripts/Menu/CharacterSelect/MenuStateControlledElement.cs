using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class MenuStateControlledElement : LoadableBehaviour {

    private CharacterSelectMenu menu;

    public List<CharacterSelectMenu.State> activeStates;

    override protected void LoadDataInternal()
    {
        if (IsActive())
        {
            gameObject.MakeVisible();
        }
        else
        {
            gameObject.MakeInvisible();
        }
    }

    public bool IsActive()
    {
        foreach(CharacterSelectMenu.State state in activeStates)
        {
            if(state == Menu.GetState())
            {
                return true;
            }
        }
        return false;
    }

    public CharacterSelectMenu Menu
    {
        get
        {
            if (menu == null)
            {
                menu = GetComponentInParent<CharacterSelectMenu>();
                menu.MenuStateSwitchedEvent -= LoadData;
                menu.MenuStateSwitchedEvent += LoadData;
                LoadData();
            }
            return menu;
        }
    }

    

}
