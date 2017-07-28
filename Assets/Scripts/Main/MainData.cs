using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton managing all the static data
public class MainData : MonoBehaviour {

    //singleton instance
    private static MainData instance;

    //inspector fields
    public SaveData saveData;
    public GameData gameData;
    public Stylesheet stylesheet;

    //unity event functions
	void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    //properties
    public static PlayerData CurrentPlayerData
    {
        get { return instance.saveData.currentPlayerData(); }
    }

    public static Stylesheet CurrentStylesheet
    {
        get { return instance.stylesheet; }
    }

    public static string PlayerName
    {
        get { return CurrentPlayerData.name; }
    }

    public static int RankPoints
    {
        get { return CurrentPlayerData.rankPoints; }
    }

    public static int Xp
    {
        get { return CurrentPlayerData.xp; }
    }


    //public functions

    //just a hack to keep game bug free
    public static void loadPlayer(GameObject player)
    {
        player.GetComponent<Xp>().xp = instance.saveData.currentPlayerData().xp;
    }

    public static void savePlayer(GameObject player)
    {
        instance.saveData.currentPlayerData().xp = player.GetComponent<Xp>().xp;
    }
}
