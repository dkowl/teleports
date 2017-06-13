using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Unit unit_;

	void Awake()
    {
        unit_ = gameObject.GetComponent<Unit>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("PlayerMove"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool done = false;

            int layerMask = 1 << 10; //testing for enemies
            if (Input.GetButtonDown("PlayerMove") && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                unit_.AttackTarget = hit.transform.parent.gameObject.GetComponent<Unit>();
                done = true;
            }

            layerMask = 1 << 8; //testing for ground
            if (!done && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                unit_.moveTo(hit.point);
                if (Input.GetButtonDown("PlayerMove"))
                {
                    gameObject.GetComponent<Unit>().resetAttack();
                }
            }
        }
	}
}
