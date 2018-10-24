using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Teleports.Utils;

public class TargetedEnemyInfo : MonoBehaviour {

    [SerializeField]
    Slider slider;
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI hpText;

    PlayerController playerController;
    Unit targetUnit;

    void Start()
    {
        playerController = Main.CurrentGameSession.PlayerGameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.Target.TargetUnit != null)
        {
            targetUnit = playerController.Target.TargetUnit;
        }

        if(targetUnit == null || targetUnit.DeadState.IsActive)
        {
            gameObject.MakeInvisible();
        }
        else
        {
            slider.value = targetUnit.HealthPercentage;
            nameText.text = targetUnit.UnitData.Name;
            hpText.text = targetUnit.CurrentHp.ToString() + " / " + targetUnit.Hp.ToString();
            gameObject.MakeVisible();
        }
    }

}
