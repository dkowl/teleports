using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : UnlockableSlotUI {

    [SerializeField]
    private int skillSlotId;

    private SkillTreeSlot skillSlot;

    protected override UnlockableSlot GetSlot()
    {
        if (Main.GameState.CurrentHeroData != null)
        {
            skillSlot = Main.GameState.CurrentHeroData.SkillTreeSlots[skillSlotId];
            return skillSlot;
        }
        else
        {
            return new UnlockableSlot();
        }
    }

    protected override void OnFull()
    {
        base.OnFull();
        text.text = "undefined";
    }

    public void SetSlotID(int id)
    {
        skillSlotId = id;
    }
}
