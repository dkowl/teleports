using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRewardPopupCommand : IUIAnimatorCommand {

    private string text;
    private string smallText;
    private bool isFinished = false;

    public ShowRewardPopupCommand(string text, string smallText = "")
    {
        this.text = text;
        this.smallText = smallText;
    }

	public virtual void Execute()
    {
        MenuController.Instance.OpenMenu(MenuController.MenuIdRewardPopup);
        var popup = 
            MenuController.Instance.GetMenu(MenuController.MenuIdRewardPopup).
            InstantiatedObject.GetComponentInChildren<RewardPopupUI>();
        popup.SetMainText(text);
        if(smallText.Length > 0) popup.AddSmallText(smallText);
        popup.SetClickCallback(() => { Skip(); });
    }

    public virtual void Update(float deltaTime) { }

    public virtual void Skip()
    {
        MenuController.Instance.CloseMenu(MenuController.MenuIdRewardPopup);
        isFinished = true;
    }

    public virtual bool IsFinished()
    {
        return isFinished;
    }


}
