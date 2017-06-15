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

    public void showDamage(float damage)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(damage.ToString());
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().height_, 0);
    }

    public void showXp(int xp)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(xp.ToString() + " XP");
        obj.GetComponent<FloatingDamage>().setColor(Color.yellow);
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().height_, 0);
    }
}
