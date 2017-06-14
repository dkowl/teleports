using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

[RequireComponent(typeof(Unit))]
public class Healthbar : MonoBehaviour {

    GameObject gameObject_;
    Unit unit_;
    public Slider slider_;

	// Use this for initialization
	void Start () {
        gameObject_ = Instantiate(Resources.Load("Prefabs/Unit/Healthbar"), gameObject.transform) as GameObject;
        unit_ = gameObject.GetComponent<Unit>();
        gameObject_.transform.localPosition = new Vector3(0, unit_.height_ + 0.3f, 0);
        slider_ = gameObject_.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider_.value = unit_.healthPercentage();
        if (!unit_.alive())
        {
            gameObject_.makeInvisible();
        }
	}
}
