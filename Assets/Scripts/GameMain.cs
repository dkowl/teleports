using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {

    public static GameMain instance;

    GameObject player_;
    GameObject mainCanvas_;

    int score_, startXp_;
    public int Score
    {
        get { return score_; }
    }

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
        player_ = GameObject.FindGameObjectWithTag("Player");
        startXp_ = player_.GetComponent<Xp>().xp;
        mainCanvas_ = Instantiate(Resources.Load("Prefabs/UI/MainCanvas"), gameObject.transform) as GameObject;

        endScreenOn_ = false;
        unpauseGame();
	}
	
	// Update is called once per frame
	void Update () {
        if (!player_.GetComponent<Unit>().alive())
        {
            endScreen();
        }
        score_ = player_.GetComponent<Xp>().xp - startXp_;
                   
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
        GlobalData.instance.savePlayer(player_);
        unpauseGame();
        SceneManager.LoadScene("Home");
    }
}
