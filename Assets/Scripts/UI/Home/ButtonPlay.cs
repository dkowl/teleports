using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Play);
	}
	
	void Play ()
    {
        SceneManager.UnloadSceneAsync("Home");
        SceneManager.LoadScene("World");
	}
}
