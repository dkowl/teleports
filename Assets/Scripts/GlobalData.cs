
using UnityEngine;

public class GlobalData : MonoBehaviour {

    public static GlobalData instance;

    public PlayerData playerData_;
    public TeleportData teleportData_;

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
        playerData_.xp = player.GetComponent<Xp>().xp;
    }

    public void loadPlayer(GameObject player)
    {
        player.GetComponent<Xp>().xp = playerData_.xp;
    }

}
