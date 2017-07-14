using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 0)]
public class PlayerData : ScriptableObject {

    static int probeLength_ = 5;

    public string name_;
    int xp_, startXp_;
    int level_;
    int rankPoints_;

    public int xp
    {
        set {
            xp_ = value;
        }
        get { return xp_; }
    }
    public int startXp
    {
        get
        {
            return startXp_;
        }

        set
        {
            startXp_ = value;
        }
    }
    public int requiredXp
    {
        get
        {
            return XpLevels.requiredXp(xp_);
        }
    }
    public int currentXp
    {
        get
        {
            return XpLevels.currentXp(xp_);
        }
    }
    public int level
    {
        get {
            updateLevel();
            return level_;
        }
    }
    public int RankPoints
    {
        get { return rankPoints_; }
    }

    void updateLevel()
    {
        level_ = XpLevels.level(xp_);
    }

    public void updateRankPoints(int score)
    {
        rankPoints_ += (score - rankPoints_) / probeLength_;
    }
}
