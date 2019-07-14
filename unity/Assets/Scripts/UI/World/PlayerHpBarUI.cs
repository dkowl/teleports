using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBarUI : MonoBehaviour {

    private Unit playerUnit;
    private BasicProgressBar bar;

    private void Start()
    {
        playerUnit = Main.CurrentGameSession.PlayerGameObject.GetComponent<Unit>();
        bar = gameObject.GetComponent<BasicProgressBar>();
        Debug.Assert(bar != null);

        bar.CustomValueInterpreter = new ProgressBarValueInterpreter_HP(playerUnit);

        bar.SetValues(new BasicProgressBar.Values
        {
            current = playerUnit.CurrentHp,
            delta = 0,
            max = playerUnit.Hp,
            min = 0,
            target = playerUnit.CurrentHp
        });
    }

    private void Update()
    {
        bar.SetValues(new BasicProgressBar.Values
        {
            current = bar.CurrentValues.current,
            delta = 0,
            max = playerUnit.Hp,
            min = 0,
            target = playerUnit.CurrentHp
        });
    }
    
}
