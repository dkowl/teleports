using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesMenuApplyButton : DialogWindowStarterButton {

    [SerializeField] private AttributesMenu parentMenu;

    protected override List<ButtonChoice> Choices()
    {
        var result = new List<ButtonChoice>();
        if (parentMenu.HasPointsToApply())
        {
            result.Add(new ButtonChoice("Yes", parentMenu.ApplySpentPoints));
            result.Add(new ButtonChoice("No", () => { }));
        }
        else
        {
            result.Add(new ButtonChoice("OK", () => { }));
        }
        return result;
    }

    protected override string TextString()
    {
        if (parentMenu.HasPointsToApply())
        {
            return parentMenu.GetApplyQuestionString();
        }
        else
        {
            return "Nothing to apply";
        }
    }


}
