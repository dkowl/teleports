using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonReturn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(GameMain.instance.backToHome);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
