using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XpLevels {

    private static int[] xpLevels = new int[] {
        0,
        50,
        120,
        250,
        420,
        650,
        1000,
        1600,
        2400,
        3300,
        4500,
        5800,
        7500,
        9500,
        12000,
        15000,
        17500,
        21000,
        24000,
        28000,
        33000,
        38000,
        44000,
        50000,
        57000,
        64000,
        72000,
        81000,
        90000,
        100000
    };

    public static int Level(int xp)
    {        
        for (int i = 0; i < xpLevels.Length; i++)
        {
            if (xpLevels[i] >= xp)  return i;
        }
        return 0;
    }

    public static int CurrentXp(int xp)
    {
        int lvl = Level(xp);
        if (lvl > 0)
            return xp - xpLevels[lvl - 1];
        else return 0;
    }

    public static int RequiredXp(int xp)
    {
        int lvl = Level(xp);
        if (lvl > 0)
            return xpLevels[lvl] - xpLevels[lvl - 1];
        else return 1;
    }

    public static int MaxLevel
    {
        get
        {
            return xpLevels.Length;
        }
    }
}
