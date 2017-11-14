using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQtest : MonoBehaviour {

    public SkinnedMeshRenderer targetMesh;
    public SkinnedMeshRenderer shirtMesh;
	// Use this for initialization
	void Start () {
        shirtMesh.transform.parent = targetMesh.transform;
        shirtMesh.bones = targetMesh.bones;
        shirtMesh.rootBone = targetMesh.rootBone;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
