using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Teleports.Utils;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Main : Singleton<Main>, ISingletonInstance
{
    [SerializeField] private LoadingGraphics loadingGraphicsConcrete;
    [SerializeField] private Persistence persistenceConcrete;

    private IMessageBus messageBus;
    private ISceneController sceneController;
    private ILoadingGraphics loadingGraphics;
    private IPersistence persistence;
    private IUISystem uiSystem;

    //cached persistence properties
    private IStaticData staticData;
    private IGameState gameState;
    private IServerData serverData;

    private IGameSession currentGameSession;

    public static event System.Action AfterInitializationEvent;

    private void Awake()
    {
        //calling to trigger initialization
        Main main = Instance;
    }

    private void OnApplicationQuit()
    {
        persistence.SaveGameState();
        DestroyAllButThis();
        Debug.Log("Game quit!");
    }

    public void StartGameSession()
    {
        currentGameSession = new GameSession();
        currentGameSession.Start(gameState);
    }

    public void EndGameSession()
    {
        currentGameSession.End(OnGameSessionEnd);
    }

    private void OnGameSessionEnd()
    {
        gameState.Update(currentGameSession.GetResult());
        currentGameSession = null;
    }

    public void OnFirstAccess()
    {
        Initialize();
    }

    [Button]
    private void Initialize()
    {
        if (!Application.isPlaying)
        {
            InitializeInEditMode();
        }
        else
        {
            InitializeInEditMode();
            InitializeInPlayMode();
        }
        Debug.Log("Main initialized!");
    }

    private void InitializeInEditMode()
    {
        persistence = persistenceConcrete;

        staticData = persistence.GetStaticData();
        gameState = persistence.GetGameState();
        serverData = persistence.GetServerData();

        messageBus = new MessageBus();

        uiSystem = new UISystem();
        uiSystem.InitInEditMode();
    }

    private void InitializeInPlayMode()
    {
        loadingGraphics = loadingGraphicsConcrete;
        InitializeInterfaceWithComponent<SceneController, ISceneController>(out sceneController);
        DontDestroyOnLoad(this);

        uiSystem.Start();
        AfterInitializationEvent?.Invoke();
    }

    private void InitializeInterfaceWithComponent<T, I>(out I i) where T : Component, I
    {
        T temp = null;
        gameObject.InitComponent(ref temp);
        i = temp;
    }

    private void DestroyAllButThis()
    {
        foreach (var gameObject in FindObjectsOfType<GameObject>())
        {
            if (this.gameObject.GetInstanceID() == gameObject.GetInstanceID()) continue;

            DestroyImmediate(gameObject);
        }
    }

    public static IMessageBus MessageBus => Instance.messageBus;
    public static ISceneController SceneController => Instance.sceneController;
    public static ILoadingGraphics LoadingGraphics => Instance.loadingGraphics;
    public static IStaticData StaticData => Instance.staticData;
    public static IGameState GameState => Instance.gameState;
    public static IServerData ServerData => Instance.serverData;
    public static IGameSession CurrentGameSession => Instance.currentGameSession;
    public static IUISystem UISystem => Instance.uiSystem;
    public static GameObject MainGameObject => Instance.gameObject;

    public static void LoadGameState()
    {
        Instance.persistence.LoadGameState();
    }

    public static void SaveGameState()
    {
        Instance.persistence.SaveGameState();
    }
}
