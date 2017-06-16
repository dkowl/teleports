using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    Text text_;

    void Awake()
    {
        text_ = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        float timeLeft = GameMain.TimeLeft;
        int
            minutes = (int)timeLeft / 60, seconds = (int)timeLeft % 60;
        text_.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
