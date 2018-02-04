using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour {

    TextMeshProUGUI text;

    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        float timeLeft = GameMain.TimeLeft;
        int minutes = Mathf.Max((int)timeLeft / 60, 0), 
            seconds = Mathf.Max((int)timeLeft % 60, 0);
        text.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
