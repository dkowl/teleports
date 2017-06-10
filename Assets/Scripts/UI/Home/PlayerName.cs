using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PlayerName : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text text = gameObject.GetComponent<Text>();
        text.text = GlobalData.instance.playerData_.name_;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
