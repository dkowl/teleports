using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

[RequireComponent(typeof(Unit))]
public class Healthbar : MonoBehaviour {

    GameObject healthbar;
    Unit unit;
    public Slider slider;

	// Use this for initialization
	void Start () {
        healthbar = Instantiate(Resources.Load("Prefabs/Unit/Healthbar"), gameObject.transform) as GameObject;
        unit = gameObject.GetComponent<Unit>();
        healthbar.transform.localPosition = new Vector3(0, unit.unitData.Height + 0.3f, 0);
        slider = healthbar.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = unit.HealthPercentage;
        if (!unit.Alive())
        {
            healthbar.makeInvisible();
        }
	}
}
