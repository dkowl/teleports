using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGamePopUpEvent_RpEarned : PostGamePopUpEvent {

    public struct Data
    {
        public int oldRp;
        public int newRp;
        public bool isStartingASequence;
        public bool isEndingASequence;
    }

    private Data data;

    public PostGamePopUpEvent_RpEarned(Data data)
    {
        type = PostGamePopUpEventType.RpEarned;
        this.data = data;
    }

    public int OldRp => data.oldRp;
    public int NewRp => data.newRp;
    public bool IsStartingASequence => data.isStartingASequence;
    public bool IsEndingASequence => data.isEndingASequence;
}
