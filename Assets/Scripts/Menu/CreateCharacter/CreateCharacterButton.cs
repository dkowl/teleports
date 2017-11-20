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
        switch (PlayerDataValidator.ValidateName(heroName))
        {
            case PlayerDataValidator.NameValidationResult.OK:
                result.Add(new ButtonChoice("Yes", menu.CreateCharacter));
                result.Add(new ButtonChoice("No", DoNothing));
                break;
            default:
                result.Add(new ButtonChoice("OK", DoNothing));
                break;
        }
        return result;
    }

    protected override string TextString()
    {
        string heroName = menu.Name;
        switch (PlayerDataValidator.ValidateName(heroName))
        {
            case PlayerDataValidator.NameValidationResult.OK:
                return "Create new hero " + heroName + "?";
            case PlayerDataValidator.NameValidationResult.TooShort:
                return "Name must be at least " + PlayerDataValidator.MinNameLength.ToString() + " characters long";
            case PlayerDataValidator.NameValidationResult.TooLong:
                return "Name must be shorter than " + PlayerDataValidator.MaxNameLength.ToString() + "characters";
            default:
                return "Unexpected name validation error";
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
