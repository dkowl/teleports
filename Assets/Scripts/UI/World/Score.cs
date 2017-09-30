using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {

    TextMeshProUGUI text;
     
	void Awake () {
        text = gameObject.GetComponent<TextMeshProUGUI>();
	}
	
	void Update () {
        text.text = GameMain.Instance.Score.ToString();
	}
}
