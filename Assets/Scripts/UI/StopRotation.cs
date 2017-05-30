using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour {

	
	// Update is called once per frame
	void LateUpdate () {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
