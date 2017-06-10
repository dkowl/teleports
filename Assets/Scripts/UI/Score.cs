using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    Text text_;
     
	// Use this for initialization
	void Start () {
        text_ = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text_.text = "Score: " + GameMain.instance.Score.ToString();
	}
}
