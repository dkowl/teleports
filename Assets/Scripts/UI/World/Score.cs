using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    Text text_;
     
	void Awake () {
        text_ = gameObject.GetComponent<Text>();
	}
	
	void Update () {
        text_.text = "Score: " + GameMain.Instance.Score.ToString();
	}
}
