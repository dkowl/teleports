using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGamePopUpEvent_RankUp : PostGamePopUpEvent
{
    private int newRank;

    public PostGamePopUpEvent_RankUp(int newRank)
    {
        type = PostGamePopUpEventType.RankUp;
        this.newRank = newRank;
    }

    public int NewRank => newRank;
}
