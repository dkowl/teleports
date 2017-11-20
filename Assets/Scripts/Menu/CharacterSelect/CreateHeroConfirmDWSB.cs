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
        return MainData.CurrentSaveData.GetPlayerData(characterSlotId) == null;
    }

    void Yes()
    {
        MainData.CurrentSaveData.CurrentSlotID = characterSlotId;
        MenuController.Instance.OpenMenu(MenuController.MenuType.CreateCharacter);
    }

    public int CharacterSlotId
    {
        set { characterSlotId = value; }
    }
}
