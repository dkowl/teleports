using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGraphics : MonoBehaviour {

    Healthbar healthbar_;

	// Use this for initialization
	void Start () {
        healthbar_ = gameObject.AddComponent<Healthbar>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void showDamage(int damage)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(damage.ToString());
    }
}
