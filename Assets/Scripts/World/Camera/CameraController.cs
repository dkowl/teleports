using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public bool followPlayer;
    public float cameraHeight;

	// Use this for initialization
	private void Start () {
        followPlayer = true;
	}
	
	// Update is called once per frame
	private void Update () {

        if (followPlayer)
        {
            Vector3 playerPos = PlayerPosition;
            Vector3 cameraPos = new Vector3();
            cameraPos.x = playerPos.x;
            cameraPos.y = cameraHeight;
            cameraPos.z = playerPos.z;
            
            float angle = 90 - transform.rotation.eulerAngles.x;
            cameraPos.z -= cameraHeight * Mathf.Tan(Mathf.Deg2Rad * angle);

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

    private Vector3 PlayerPosition
    {
        get
        {
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
            return playerGameObject != null ? playerGameObject.transform.position : Vector3.zero;
        }
    }
}
