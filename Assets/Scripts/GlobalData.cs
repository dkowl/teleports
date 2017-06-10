
using UnityEngine;

public class GlobalData : MonoBehaviour {

    public static GlobalData instance;

    public PlayerData playerData_;

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


    public void savePlayer(GameObject player_)
    {
        playerData_.xp += player_.GetComponent<Xp>().xp;
    }

    public void loadPlayer(GameObject player_)
    {
    }

}
