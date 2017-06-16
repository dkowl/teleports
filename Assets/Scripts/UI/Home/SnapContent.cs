using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapContent : MonoBehaviour {

    public float snapToMultiplesOf_;
    public GameObject content_;
    public float snapStrength_, tippingPoint_, maxSpeed_;
    bool isDragged_;

	public void startDrag()
    {
        isDragged_ = true;
    }

    public void endDrag()
    {
        isDragged_ = false;
    }
	
	void Update () {

        float posX = content_.transform.localPosition.x;

        float closestMult;
        for(closestMult = 0; closestMult >= -10000; closestMult -= snapToMultiplesOf_)
        {
            if (Mathf.Abs(posX - closestMult) <= snapToMultiplesOf_ / 2f) break;
        }

        if (!isDragged_)
        {
            Vector3 newPos = content_.transform.localPosition;
            float distance = Mathf.Abs(newPos.x - closestMult);
            float distanceSigned = newPos.x - closestMult;
             newPos.x = Mathf.Lerp(posX, closestMult, Time.deltaTime * Mathf.Min(maxSpeed_ / distance, snapStrength_ * tippingPoint_ / (distance * distance)));
            //newPos.x = Mathf.Lerp(posX, closestMult, Time.deltaTime * snapStrength_);
            content_.transform.localPosition = newPos;
            
 
        }
	}
}
