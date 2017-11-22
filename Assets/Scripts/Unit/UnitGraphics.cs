using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGraphics : MonoBehaviour {

    Healthbar healthbar;
    GameObject targetMarker;

	// Use this for initialization
	void Start () {
        healthbar = gameObject.AddComponent<Healthbar>();
    }

    public void updateTarget(Unit target)
    {
        if(targetMarker == null)
        {
            targetMarker = Instantiate(Resources.Load("Prefabs/Unit/TargetMarker"), gameObject.transform) as GameObject;
        }
        targetMarker.GetComponent<TargetMarker>().setTargetUnit(target);
    }

    public void showDamage(float damage)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(damage.ToString());
        obj.GetComponent<FloatingDamage>().setColor(Color.red);
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }

    public void showXp(int xp)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(xp.ToString() + " XP");
        obj.GetComponent<FloatingDamage>().setColor(Color.yellow);
        obj.GetComponent<FloatingDamage>().lifetime *= 2;
        obj.GetComponent<FloatingDamage>().gravityY /= 2;
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }

    public void showMessage(string message)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Unit/FloatingDamage"), gameObject.transform) as GameObject;
        obj.GetComponent<FloatingDamage>().setText(message);
        obj.GetComponent<FloatingDamage>().setColor(Color.yellow);
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }
}
