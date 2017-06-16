using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

[ExecuteInEditMode]
public class ObjectPositioner : MonoBehaviour {

    public float x_, y_, z_, scale_;

	void Update () {

        Camera camera = Camera.main;

        Vector3 finalPos, viewportPos, scale;

        viewportPos = new Vector3(x_, y_, z_);
        finalPos = camera.ViewportToWorldPoint(viewportPos);

        scale = new Vector3(scale_, scale_, scale_) * camera.aspect/(Utils.baseAspect);


        gameObject.transform.position = finalPos;
        gameObject.transform.localScale = scale;
	}
}
