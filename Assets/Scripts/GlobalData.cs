
using UnityEngine;

public class GlobalData : MonoBehaviour {

    public static GlobalData instance;

    public GameData gameData_;

    public AccountState accountData_;
    public IPlayerData playerData_;
    public TeleportData teleportData_;

    //Properties
    public static AccountState Account
    {
        get { return instance.accountData_; }
        set { instance.accountData_ = value; }
    }

    //making sure only one instance exists
    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void savePlayer(GameObject player)
    {
        playerData_.Xp = player.GetComponent<Xp>().xp;
    }

    public void loadPlayer(GameObject player)
    {
        player.GetComponent<Xp>().xp = playerData_.Xp;
    }

}
