using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages adding and unlocking skill tree slots of the player
public class SkillPointManager{

    private PlayerState playerState_;

	public SkillPointManager(PlayerState playerState)
    {
        playerState_ = playerState;
    }
}
