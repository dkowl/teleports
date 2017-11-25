using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController {
	
	// Update is called once per frame
	public override void Control () {
        if (Input.GetButton("PlayerMove"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool done = false;

            int layerMask = (int)LayerMask.Enemy; //testing for enemies
            if (Input.GetButtonDown("PlayerMove") && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                target.TargetUnit = hit.transform.parent.gameObject.GetComponent<Unit>();
                done = true;
            }

            layerMask = (int)LayerMask.Ground; //testing for ground
            if (!done && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                unit.MovingState.Start(hit.point);
                if (Input.GetButtonDown("PlayerMove"))
                {
                    unit.CastingState.Reset();
                    target.TargetUnit = null;
                }
            }
        }

        if (target.TargetUnit != null) Chase();

        unit.Graphics.updateTarget(target.TargetUnit);
	}    
}
