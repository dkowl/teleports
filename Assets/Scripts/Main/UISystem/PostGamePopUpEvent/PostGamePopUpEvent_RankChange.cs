using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGamePopUpEvent_RankChange : PostGamePopUpEvent
{
    private int oldRank;
    private int newRank;

    public PostGamePopUpEvent_RankChange(int oldRank, int newRank)
    {
        type = PostGamePopUpEventType.RankChange;
        this.oldRank = oldRank;
        this.newRank = newRank;
    }

    public int OldRank => oldRank;
    public int NewRank => newRank;
    public bool IsPositive => newRank > oldRank;
}
