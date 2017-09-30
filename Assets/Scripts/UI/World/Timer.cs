using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour {

    TextMeshProUGUI text;

    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        float timeLeft = GameMain.TimeLeft;
        int
            minutes = (int)timeLeft / 60, seconds = (int)timeLeft % 60;
        text.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
