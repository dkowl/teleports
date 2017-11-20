using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBarUI : BaseProgressBarUI {

    Unit playerUnit;

    void Start()
    {
        playerUnit = GameMain.Instance.Player.GetComponent<Unit>();
        DetectChange();
        SkipAnimation();
    }

    protected override string NameTextString()
    {
        if (playerUnit != null)
            return playerUnit.unitData.Name;
        else
            return "Player";
    }

    protected override float CurrentValue()
    {
        if (playerUnit != null)
            return playerUnit.CurrentHp;
        else return 0;
    }

    protected override float MaxValue()
    {
        if (playerUnit != null)
            return playerUnit.Hp;
        else
            return 100;
    }
}
