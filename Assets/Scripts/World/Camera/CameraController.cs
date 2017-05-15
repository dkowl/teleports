using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 offset = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.DownArrow))
        {
            offset.z--;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            offset.z++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            offset.x--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            offset.x++;
        }

        transform.position += offset;
    }
}
