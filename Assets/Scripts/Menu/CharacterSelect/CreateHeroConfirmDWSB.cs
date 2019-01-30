using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHeroConfirmDWSB : DialogWindowStarterButton {

    int characterSlotId;

    protected override List<ButtonChoice> Choices()
    {
        var result = new List<ButtonChoice>();
        result.Add(new ButtonChoice("Yes", Yes));
        result.Add(new ButtonChoice("No", delegate { }));
        return result;
    }

    protected override string TextString()
    {
        return "Create new hero?";
    }

    protected override bool IsActive()
    {
        return Main.GameState.GetHeroData(characterSlotId) == null;
    }

    void Yes()
    {
        Main.GameState.SelectHero(characterSlotId);
        MenuController.Instance.OpenMenu(MenuController.MenuIdCreateHero);
    }

    public int CharacterSlotId
    {
        set { characterSlotId = value; }
    }
}
