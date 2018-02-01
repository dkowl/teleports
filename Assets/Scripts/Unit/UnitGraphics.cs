using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class UnitGraphics : MonoBehaviour {
    
    GameObject targetMarker;
    GameObject raceModel;
    GameObject ragdoll;
    Unit unit;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<Healthbar>();
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if(unit.ActiveController is PlayerController) UpdateTarget(unit.ActiveController.Target.TargetUnit);
    }

    public void UpdateTarget(Unit target)
    {
        if(targetMarker == null)
        {
            targetMarker = Instantiate(Resources.Load("Prefabs/Unit/TargetMarker"), gameObject.transform) as GameObject;
        }
        targetMarker.GetComponent<TargetMarker>().SetTargetUnit(target);
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

    public void SwitchToRagdoll()
    {
        GameObject ragdoll = MainData.Game.GetRace(unit.UnitData.RaceName).Graphics.RagdollObject;
        if (ragdoll != null)
        {
            ragdoll = GameObject.Instantiate(ragdoll, unit.transform);
            Destroy(raceModel);
            unit.Rigidbody = ragdoll.GetComponentInChildren<Rigidbody>();
        }
        else
        {
            unit.Rigidbody.constraints = 0;
            unit.Rigidbody.useGravity = true;
        }
        unit.gameObject.SetLayerIncludingChildren(0);
    }

    public GameObject RaceModel
    {
        get { return raceModel; }
        set { raceModel = value; }
    }
}
