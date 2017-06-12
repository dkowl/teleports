using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpLevels {

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

    public static int level(int xp)
    {
        int i = 0;
        for (; i < xpLevels_.Length; i++)
        {
            if (xpLevels_[i] > xp) break;
        }
        return i;
    }

    public static int currentXp(int xp)
    {
        int lvl = level(xp);
        if (lvl > 0)
            return xp - xpLevels_[lvl - 1];
        else return 0;
    }

    public static int requiredXp(int xp)
    {
        int lvl = level(xp);
        if (lvl > 0)
            return xpLevels_[lvl] - xpLevels_[lvl - 1];
        else return 0;
    }
}
