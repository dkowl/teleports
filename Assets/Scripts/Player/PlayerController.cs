using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Unit unit_;
    public Skill mainAttack_;
    Skill.TargetInfo target_;

	void Awake()
    {
        unit_ = gameObject.GetComponent<Unit>();
        target_ = new Skill.TargetInfo();
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

    void chase()
    {
        if (unit_.canReachCastTarget(mainAttack_, target_))
        {
            unit_.cast(mainAttack_, target_);
        }
        else
        {
            unit_.moveTo(target_.position);
        }
    }
}
