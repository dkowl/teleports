using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Unit))]
public class Healthbar : MonoBehaviour {

    GameObject gameObject_;
    public Slider slider_;

	// Use this for initialization
	void Start () {
        gameObject_ = Instantiate(Resources.Load("Prefabs/Unit/Healthbar"), gameObject.transform) as GameObject;
        gameObject_.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().heigth_ + 0.3f, 0);
        slider_ = gameObject_.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider_.value = gameObject.GetComponent<Unit>().healthPercentage();
	}
}
