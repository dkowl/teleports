using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterButton : DialogWindowStarterButton {

    [SerializeField] CreateCharacterMenu menu;

    protected override List<ButtonChoice> Choices()
    {
        string heroName = menu.Name;
        var result = new List<ButtonChoice>();
        if (!PlayerData.ValidateName(heroName))
        {
            result.Add(new ButtonChoice("OK", DoNothing));
        }
        else
        {
            result.Add(new ButtonChoice("Yes", menu.CreateCharacter));
            result.Add(new ButtonChoice("No", DoNothing));
        }
        return result;
    }

    protected override string TextString()
    {
        string heroName = menu.Name;
        if (!PlayerData.ValidateName(heroName))
        {
            return "Name must be at least 3 characters long";
        }
        else
        {
            return "Create new hero " + heroName + "?";
        }
    }

    void DoNothing()
    {

    }

    void Yes()
    {
        menu.CreateCharacter();
    }
}
