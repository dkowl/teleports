using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController {
	
	// Update is called once per frame
	public override void control () {
        if (Input.GetButton("PlayerMove"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool done = false;

            int layerMask = 1 << 10; //testing for enemies
            if (Input.GetButtonDown("PlayerMove") && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                target_.unit = hit.transform.parent.gameObject.GetComponent<Unit>();
                done = true;
            }

            layerMask = 1 << 8; //testing for ground
            if (!done && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                unit_.moveTo(hit.point);
                if (Input.GetButtonDown("PlayerMove"))
                {
                    unit_.resetCast();
                    target_.unit = null;
                }
            }
        }

        if (target_.unit != null) chase();

        unit_.Graphics.updateTarget(target_.unit);
	}    
}
