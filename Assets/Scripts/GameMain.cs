using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {

    private static GameMain instance;

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

        MenuController.Instance.CloseAll();

        SpawnPlayer();

        startXp = player.GetComponent<XpComponent>().Xp;
        mainCanvas = Instantiate(Resources.Load("Prefabs/UI/MainCanvas"), gameObject.transform) as GameObject;

        endScreenOn = false;
        isPaused = false;
        gameTime = 0;
    }

    
    void Start () {
        UnpauseGame();
        teleportTime = MainData.CurrentPlayerData.CurrentTeleportData.Time;
    }
	
	void Update () {

        gameTime += Time.deltaTime;

        if (gameTime >= teleportTime)
        {
            EndScreen();
        }

        if (!player.GetComponent<Unit>().Alive())
        {
            EndScreen();
        }

        score = player.GetComponent<XpComponent>().Xp - startXp;
                   
	}

    private void EndScreen()
    {
        if (!endScreenOn)
        {
            PauseGame();
            endScreen = Instantiate(Resources.Load("Prefabs/UI/EndScreen"), mainCanvas.transform) as GameObject;
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
        if(player != null) { return; }

        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            player = new GameObject("Player");
            player.transform.parent = transform;
            player.tag = "Player";
        }

        IPlayerData playerData = MainData.CurrentPlayerData;
        Unit unit = player.GetComponent<Unit>();
        PlayerController controller = player.GetComponent<PlayerController>();
        XpComponent xp = player.GetComponent<XpComponent>();

        if (unit == null)
        {
            unit = player.AddComponent<Unit>();
        }
        unit.unitData = playerData.BaseUnitData;

        if (controller == null)
        {
            controller = player.AddComponent<PlayerController>();
        }

        //Instantiating skills
        GameObject skills = new GameObject("Skills");
        skills.transform.parent = player.transform;

        GameObject primarySkill = Instantiate(MainData.CurrentGameData.GetSkill(playerData.PrimarySkillId).gameObject, skills.transform);
        controller.MainAttack = primarySkill.GetComponent<Skill>();
        unit.ActiveController = controller;

        if(xp == null)
        {
            xp = player.AddComponent<XpComponent>();
        }
        xp.Xp = playerData.Xp;

        RaceGraphics raceGraphics = MainData.CurrentGameData.GetRace(playerData.RaceName).Graphics;
        GameObject playerModel = Instantiate(raceGraphics.ModelObject, player.transform);
        playerModel.transform.localEulerAngles = Vector3.zero;

        Animator animator = playerModel.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = raceGraphics.WorldAnimationController;
        animator.gameObject.AddComponent<UnitAnimator>();

        player.AddComponent<PlayerWorldUI>();
       
    }

    public void BackToHome()
    {
        MainData.SavePlayer(player);
        MenuController.Instance.OpenMenu(MenuController.MenuType.ChooseCharacter);
        MainController.SwitchScene("Test");
    }

    public static GameMain Instance
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
