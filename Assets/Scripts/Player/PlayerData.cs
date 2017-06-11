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
    public int requiredXp
    {
        get
        {
            if (level_ > 0)
                return xpLevels_[level_] - xpLevels_[level_ - 1];
            else return 0;
        }
    }
    public int currentXp
    {
        get
        {
            if (level_ > 0)
                return xp_ - xpLevels_[level_ - 1];
            else return 0;
        }
    }
    static int[] xpLevels_ = new int[] {
        0,
        400,
        1000,
        2200,
        5000,
        10000,
        18000,
        30000,
        46000,
        70000,
        100000
    };

    int level_;
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
            if (xpLevels_[i] >= xp_) break;
        }
        level_ = i;
    }
}
