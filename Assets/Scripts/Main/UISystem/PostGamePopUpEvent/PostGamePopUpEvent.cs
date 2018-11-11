using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PostGamePopUpEventType
{
    XpEarned,
    RpEarned,
    LevelUp,
    RankUp
}

public class PostGamePopUpEvent {

    protected PostGamePopUpEventType type;

    public PostGamePopUpEventType Type => type;
}
