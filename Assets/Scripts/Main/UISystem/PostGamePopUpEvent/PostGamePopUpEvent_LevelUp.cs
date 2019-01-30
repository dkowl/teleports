using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGamePopUpEvent_LevelUp : PostGamePopUpEvent
{

    private int newLevel;
    private int attributePointsAdded;

    public PostGamePopUpEvent_LevelUp(int newLevel, int attributePointsAdded)
    {
        type = PostGamePopUpEventType.LevelUp;
        this.newLevel = newLevel;
        this.attributePointsAdded = attributePointsAdded;
    }

    public int NewLevel => newLevel;
    public int AttributePointsAdded => attributePointsAdded;
}
