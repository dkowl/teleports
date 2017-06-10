using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 0)]
public class PlayerData : ScriptableObject {

    public string name_;
    int xp_;
    public int xp
    {
        set {
            xp_ = value;
        }
        get { return xp_; }
    }
    int level_;
    static int[] xpLevels_ = new int[] {
        300,
        1000,
        3000,
        7000,
        15000,
        25000,
        35000,
        50000,
        70000,
        100000
    };

    public int level
    {
        get {
            updateLevel();
            return level_;
        }
    }

    void updateLevel()
    {
        int i = 0;
        for(; i<xpLevels_.Length; i++)
        {
            if (xpLevels_[i] > xp_) break;
        }
        level_ = i+1;
    }
}
