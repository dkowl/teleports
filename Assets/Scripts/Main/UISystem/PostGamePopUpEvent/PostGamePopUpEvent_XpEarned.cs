using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGamePopUpEvent_XpEarned : PostGamePopUpEvent
{
    public struct Data
    {
        public int oldXp;
        public int newXp;
        public bool isStartingASequence;
        public bool isEndingASequence;
    }

    private Data data;

    public PostGamePopUpEvent_XpEarned(Data data)
    {
        this.data = data;
    }

    public int OldXp => data.oldXp;
    public int NewXp => data.newXp;
    public bool IsStartingASequence => data.isStartingASequence;
    public bool IsEndingASequence => data.isEndingASequence;

}
