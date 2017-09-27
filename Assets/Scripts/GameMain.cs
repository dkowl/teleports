using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {

    public static GameMain instance;

    GameObject player;
    GameObject mainCanvas;

    float gameTime;
    float teleportTime;

    int score, startXp;

    bool endScreenOn;
    GameObject endScreen;

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

        startXp = player.GetComponent<Xp>().xp;
        mainCanvas = Instantiate(Resources.Load("Prefabs/UI/MainCanvas"), gameObject.transform) as GameObject;

        endScreenOn = false;
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

        score = player.GetComponent<Xp>().xp - startXp;
                   
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
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    private void SpawnPlayer()
    {
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
        Xp xp = player.GetComponent<Xp>();

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

        GameObject primarySkill = Instantiate(MainData.CurrentGameData.CurrentSkillDatabase.GetSkill(playerData.PrimarySkillId).gameObject, skills.transform);
        controller.MainAttack = primarySkill.GetComponent<Skill>();
        unit.ActiveController = controller;

        if(xp == null)
        {
            xp = player.AddComponent<Xp>();
        }
        xp.xp = playerData.Xp;

        RaceGraphics raceGraphics = MainData.CurrentGameData.GetRace(playerData.RaceName).Graphics;
        GameObject playerModel = Instantiate(raceGraphics.ModelObject, player.transform);
        playerModel.transform.localEulerAngles = Vector3.zero;

        Animator animator = playerModel.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = raceGraphics.WorldAnimationController;
        animator.gameObject.AddComponent<UnitAnimator>();
       
    }

    public void BackToHome()
    {
        MainData.SavePlayer(player);
        UnpauseGame();
        MenuController.Instance.OpenMenu(MenuController.MenuType.ChooseCharacter);
        MainController.SwitchScene("Test");
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
