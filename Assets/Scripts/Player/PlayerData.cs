using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 0)]
public class PlayerData : ScriptableObject {

    public string name_;
    public int xp_;

    public int level()
    {
        return 1;
    }
}
