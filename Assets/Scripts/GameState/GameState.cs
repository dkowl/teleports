using System.Collections;


[System.Serializable]
public class GameState {

    private bool empty_ = true;

    public bool Empty
    {
        get { return empty_; }
    }

    PlayerState playerState_;
    TeleportState teleportState_;
}
