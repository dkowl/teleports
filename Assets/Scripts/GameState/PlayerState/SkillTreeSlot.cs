using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents a slot to put a skill tree in
public class SkillTreeSlot : MonoBehaviour
{

    private bool
        isEmpty_,
        isLocked_;

    private SkillTree skillTree_;

    private int pointsInvested_;

    public SkillTreeSlot()
    {
        isEmpty_ = true;
        isLocked_ = false;

        pointsInvested_ = 0;
    }
}
