using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : LoadableBehaviour {

    public int skillSlotId;
    public Image image;
    public Text text;

    public Stylesheet.ColorPreset lockedColor;
    public Stylesheet.ColorPreset unlockedColor;

    public override void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;

        SkillTreeSlot slot = MainData.CurrentPlayerData.GetSkillTreeSlot(skillSlotId);
        string textString;
        if (slot.IsLocked)
        {
            textString = "Locked";
            image.color = stylesheet.GetColorPreset(lockedColor);
        }
        else if (slot.IsEmpty)
        {
            textString = "Empty";
            image.color = stylesheet.GetColorPreset(unlockedColor);
        }
        else
        {
            textString = MainData.CurrentPlayerData.GetSkillTreeSlotLevel(skillSlotId).ToString();
            image.color = stylesheet.GetColorPreset(unlockedColor);
        }
        text.text = textString;
    }
}
