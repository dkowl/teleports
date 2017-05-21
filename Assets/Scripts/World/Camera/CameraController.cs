using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public bool followPlayer_;
    public float cameraHeight_;

	// Use this for initialization
	void Start () {
        followPlayer_ = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (followPlayer_)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 cameraPos = new Vector3();
            cameraPos.x = playerPos.x;
            cameraPos.y = cameraHeight_;
            cameraPos.z = playerPos.z;

            Camera camera = gameObject.GetComponent<Camera>();
            float angle = 90 - transform.rotation.eulerAngles.x;
            cameraPos.z -= cameraHeight_ * Mathf.Tan(Mathf.Deg2Rad * angle);

            transform.position = cameraPos;
        }

        else
        {
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
}
