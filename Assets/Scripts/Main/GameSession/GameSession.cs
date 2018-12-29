using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameSession : IGameSession
{
    private IGameState cachedGameState;
    private string enteringSceneName;

    private GameObject mainGameObject;
    private GameObject playerGameObject;
    private IWorld world;
    private EnemySpawner enemySpawner;
    private float timeElapsed;
    private float gameEndTime;
    private int xpEarned;

    private HUD hud;

    private System.Action onEndAction;

    public void Start(IGameState gameState)
    {
        cachedGameState = gameState;

        enteringSceneName = Main.SceneController.CurrentSceneName;
        MenuController.Instance.HideAll();
        Main.SceneController.SwitchSceneThenInvoke("World", StartAfterSceneLoad);
    }

    public void StartAfterSceneLoad()
    {
        mainGameObject = SpawnMainGameObject();
        world = SpawnWorld();
        playerGameObject = SpawnPlayer();
        enemySpawner = SpawnEnemySpawner();
        timeElapsed = 0;
        gameEndTime = cachedGameState.CurrentHeroData.TeleportData.Time;

        hud = new HUD();
        hud.Spawn(mainGameObject);

        world.Update(playerGameObject.transform.position);
    }

    public IGameSessionResult GetResult()
    {
        return new GameSessionResult
        {
            xpEarned = xpEarned
        };
    }

    public void Update(float deltaTime)
    {
        timeElapsed += deltaTime;
        world.Update(playerGameObject.transform.position);
        xpEarned = playerGameObject.GetComponent<XpComponent>().Xp;
        CheckForGameEnd();
    }

    public void End(System.Action onEndAction)
    {
        hud.Despawn();
        this.onEndAction = onEndAction;
        Main.SceneController.SwitchSceneThenInvoke(enteringSceneName, EndAfterSceneLoad);
    }

    public void EndAfterSceneLoad()
    {
        MenuController.Instance.OpenMenu(MenuController.MenuType.Home);
        onEndAction?.Invoke();
    }

    private void CheckForGameEnd()
    {
        if (timeElapsed > gameEndTime)
        {
            playerGameObject.GetComponent<Unit>().Kill();
            hud.EndScreen("Time's up!");
        }

        if (!playerGameObject.GetComponent<Unit>().Alive)
        {
            hud.EndScreen("You died!");
        }
    }

    private GameObject SpawnMainGameObject()
    {
        var result = new GameObject("Game Session");
        var updater = result.AddComponent<MonoBehaviourUpdater>();
        updater.onUpdate = Update;
        return result;
    }

    private IWorld SpawnWorld()
    {
        World result = new World();
        result.Spawn(GenerateWorldCreationParams(cachedGameState));
        return result;
    }

    private GameObject SpawnPlayer()
    {
        return PlayerSpawner.Spawn(mainGameObject);
    }

    private EnemySpawner SpawnEnemySpawner()
    {
        var enemySpawnerGameObject = new GameObject("Enemy Spawner");
        enemySpawnerGameObject.transform.parent = mainGameObject.transform;
        var result = enemySpawnerGameObject.AddComponent<EnemySpawner>();
        result.PlayerGameObject = playerGameObject;
        return result;
    }

    private static IWorldCreationParams GenerateWorldCreationParams(IGameState gameState)
    {
        var result = new WorldCreationParams
        {
            seed = Random.Range(int.MinValue, int.MaxValue),
            worldData = Main.StaticData.Game.Worlds.RandomValue
        };
        return result;
    }

    public GameObject PlayerGameObject => playerGameObject;
    public float TimeLeft => gameEndTime - timeElapsed;
}
