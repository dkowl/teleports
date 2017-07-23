using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 0)]
public class PlayerData : ScriptableObject {

    //main attributes
    public string characterName;
    public int xp;
    public int rankPoints;
    public SkillID[] skills;
	
}
