using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//singleton managing all the static data
public partial class MainData : MonoBehaviour {

    //singleton instance
    private static MainData instance;

    //inspector fields
    [SerializeField]
    private SaveData saveData;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private ServerData serverData;
    [SerializeField]
    private Stylesheet stylesheet;

    //unity event functions
	void Awake()
    {
        instance = this;
        Debug.Log("MainData initialized!");
    }

    void OnDestroy()
    {
        instance = null;
    }

    //editor only
    void OnEnable()
    {
        if(Application.isEditor) Awake();
    }

    //properties

    public static IPlayerData CurrentPlayerData
    {
        get { return instance.saveData.currentPlayerData(); }
    }

    public static GameData CurrentGameData
    {
        get { return instance.gameData; }
    }

    public static IServerData CurrentServerData
    {
        get { return instance.serverData; }
    }

    public static Stylesheet CurrentStylesheet
    {
        get { return instance.stylesheet; }
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


    //public functions

    //just a hack to keep game bug free
    public static void loadPlayer(GameObject player)
    {
        player.GetComponent<Xp>().xp = instance.saveData.currentPlayerData().Xp;
    }

    public static void savePlayer(GameObject player)
    {
        instance.saveData.currentPlayerData().Xp = player.GetComponent<Xp>().xp;
    }
}
