using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PostGamePopUpEventType
{
    XpEarned,
    RpEarned,
    LevelUp,
    RankChange
}

public class PostGamePopUpEvent {

    protected PostGamePopUpEventType type;
    protected Command uiCommand;

    public PostGamePopUpEventType Type => type;
    public Command UiCommand => uiCommand;
}
