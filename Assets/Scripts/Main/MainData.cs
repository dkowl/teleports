using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//singleton managing all the static data
[CreateAssetMenu(fileName = "mainData", menuName = "Data/Main")]
public partial class MainData : ScriptableObject {

    //singleton instance
    public static MainData instance;
    static bool isInitialized = false;

    //inspector fields
    [SerializeField] private SaveDataSO saveData;
    [SerializeField] private GameData gameData;
    [SerializeField] private ServerData serverData;
    [SerializeField] private Stylesheet stylesheet;
    [SerializeField] private DataDefaults defaults;

    IMessageBus messageBus;

    public delegate void OnInitialized();
    public static event OnInitialized OnInitializedEvent;

    //unity event functions
    void Awake()
    {
        Initialize();
    }

    void OnDestroy()
    {
        instance = null;
    }

    //editor only
    void OnEnable()
    {
        Initialize();
    }


    //public functions
    void Initialize()
    {
        if (!isInitialized || instance == null)
        {
            instance = this;
            Save.CorrectInvalidData();
            messageBus = new MessageBus();

            isInitialized = true;
            if (OnInitializedEvent != null)
            {
                OnInitializedEvent();
            }
            Debug.Log("MainData initialized!");
        }
    }

    public static void SavePlayer(GameObject player)
    {
        CurrentPlayerData.Xp = player.GetComponent<XpComponent>().Xp;
    }

    #region properties
    private static MainData Instance{
        get {
            if (instance == null)
            {
                instance = Resources.Load("Data/mainData") as MainData;
            }
            return instance;
        }
    }

    public static SaveData Save
    {
        get { return Instance.saveData.saveData; }
    }

    public static SaveDataSO SaveSO
    {
        get { return Instance.saveData; }
    }

    public static IPlayerData CurrentPlayerData
    {
        get { return Save.CurrentPlayerData(); }
    }

    public static GameData Game
    {
        get { return Instance.gameData; }
    }

    public static IServerData CurrentServerData
    {
        get { return Instance.serverData; }
    }

    public static Stylesheet Stylesheet
    {
        get { return Instance.stylesheet; }
    }

    public static DataDefaults Defaults
    {
        get { return Instance.defaults; }
    }

    public static IMessageBus MessageBus
    {
        get { return Instance.messageBus; }
    }

    public static string PlayerName
    {
        get { return CurrentPlayerData.CharacterName; }
    }

    public static int RankPoints
    {
        get { return CurrentPlayerData.RankPoints; }
    }

    public static int Xp
    {
        get { return CurrentPlayerData.Xp; }
    }
    #endregion
}
