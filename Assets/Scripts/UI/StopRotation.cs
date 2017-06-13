using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
        gameObject.transform.rotation = Quaternion.Euler(45, 0, 0);
	}
}
