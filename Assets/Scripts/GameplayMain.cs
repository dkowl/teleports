using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayMain : MonoBehaviour {

    private static GameplayMain instance;

    public Camera uiCamera;

    GameObject player;
    GameObject mainCanvas;

    float gameTime;
    float teleportTime;

    int score, startXp;

    bool endScreenOn;
    GameObject endScreen;

    bool isPaused;

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

        MenuController.Instance.HideAll();

        SpawnPlayer();

        startXp = player.GetComponent<XpComponent>().Xp;
        mainCanvas = Instantiate(Resources.Load("Prefabs/UI/MainCanvas"), gameObject.transform) as GameObject;

        endScreenOn = false;
        isPaused = false;
        gameTime = 0;
    }

    
    void Start () {
        UnpauseGame();
        teleportTime = MainData.CurrentPlayerData.TeleportData.Time;
        Debug.LogFormat("Teleport.Time is {0}", teleportTime);
    }
	
	void Update () {

        gameTime += Time.deltaTime;

        if (gameTime >= teleportTime)
        {
            player.GetComponent<Unit>().Kill();
            EndScreen("Time's up!");
        }

        if (!player.GetComponent<Unit>().Alive)
        {
            EndScreen("You died!");
        }

        score = player.GetComponent<XpComponent>().Xp - startXp;
                   
	}

    private void EndScreen(string text)
    {
        if (!endScreenOn)
        {
            PauseGame();
            endScreen = Instantiate(Resources.Load("Prefabs/UI/EndScreen"), mainCanvas.transform) as GameObject;
            endScreen.GetComponent<EndScreenUI>().SetText(text);
            endScreenOn = true;
        }
    }

    private void PauseGame()
    {
        isPaused = true;
    }

    private void UnpauseGame()
    {
        isPaused = false;
    }

    private void SpawnPlayer()
    {
        //try finding player object, if not found, spawn new one
        if(player != null) { return; }

        player = PlayerSpawner.Spawn(gameObject);
       
    }

    public void BackToHome()
    {
        MainData.SavePlayer(player);
        MainData.MessageBus.Publish(new RunFinishedMessage(Score));
        MenuController.Instance.OpenMenu(MenuController.MenuType.Home);
        SceneController.SwitchScene(SceneNames.Home);
    }

    public static GameplayMain Instance
    {
        get { return instance; }
    }

    public GameObject Player
    {
        get
        {
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                if(player == null)
                {
                    SpawnPlayer();
                }
            }
            return player;
        }
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }

    static public float TimeLeft
    {
        get { return instance.teleportTime - instance.gameTime; }
    }

    public int Score
    {
        get { return score; }
    }
}
