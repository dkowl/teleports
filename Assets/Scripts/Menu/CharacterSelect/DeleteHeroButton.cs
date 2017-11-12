using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHeroButton : DialogWindowStarterButton {

    protected override List<ButtonChoice> Choices()
    {
        List<ButtonChoice> choices = new List<ButtonChoice>();
        choices.Add(new ButtonChoice("Yes", Yes));
        choices.Add(new ButtonChoice("No", No));

        return choices;
    }

    protected override string TextString()
    {
        return "Are you sure you want to delete " + MainData.CurrentPlayerData.CharacterName + "?";
    }

    protected override bool IsActive()
    {
        return MainData.CurrentPlayerData != null;
    }

    public void Yes()
    {
        MainData.CurrentSaveData.DeleteCurrentPlayer();
    }

    public void No()
    {

    }
	
}
