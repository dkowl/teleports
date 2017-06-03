using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {

    public static GameMain instance;

    GameObject[] players_;
    GameObject mainCanvas_;

    bool endScreenOn_;
    GameObject endScreen_;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start () {
        players_ = GameObject.FindGameObjectsWithTag("Player");
        mainCanvas_ = Instantiate(Resources.Load("Prefabs/UI/MainCanvas"), gameObject.transform) as GameObject;

        endScreenOn_ = false;
        unpauseGame();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject player in players_)
        {
            if (!player.GetComponent<Unit>().alive())
            {
                endScreen();
            }

        }
	}

    void endScreen()
    {
        if (!endScreenOn_)
        {
            pauseGame();
            endScreen_ = Instantiate(Resources.Load("Prefabs/UI/EndScreen"), mainCanvas_.transform) as GameObject;
            endScreenOn_ = true;
        }
    }

    void pauseGame()
    {
        Time.timeScale = 0;
    }

    void unpauseGame()
    {
        Time.timeScale = 1;
    }

    public void backToHome()
    {
        SceneManager.UnloadSceneAsync("World");
        SceneManager.LoadScene("Home");
    }
}
