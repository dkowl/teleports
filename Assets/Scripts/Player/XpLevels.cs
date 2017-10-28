using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels {

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

    public static Levels xp = new Levels(xpLevels);

    private int[] levels;   
    
    public Levels(int[] levels)
    {
        this.levels = levels;
    } 

    public int Level(int xp)
    {        
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] >= xp)  return i;
        }
        return 0;
    }

    public int Current(int xp)
    {
        int lvl = Level(xp);
        if (lvl > 0)
            return xp - levels[lvl - 1];
        else return 0;
    }

    public int Required(int xp)
    {
        int lvl = Level(xp);
        if (lvl > 0)
            return levels[lvl] - levels[lvl - 1];
        else return 1;
    }

    public float Progress(int xp)
    {
        return (float)Current(xp) / Required(xp);
    }

    public int MaxLevel
    {
        get
        {
            return levels.Length;
        }
    }


}
