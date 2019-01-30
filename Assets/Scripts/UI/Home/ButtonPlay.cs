using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour {
    
	void Start () {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Play);
	}
	
	void Play () {
        Main.Instance.StartGameSession();
	}
}
