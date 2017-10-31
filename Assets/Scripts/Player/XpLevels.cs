using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels {

    private static int[] xpLevels = new int[] {
        0,
        138,
        690,
        1931,
        4000,
        7103,
        11448,
        17724,
        26276,
        37448,
        52621,
        72345,
        97172,
        131241,
        175655,
        231517,
        304345,
        395793,
        507517,
        656897,
        847241,
        1081862,
        1385310,
        1762828,
        2219655,
        2812759,
        3551517,
        4445310,
        5603931,
        7043931
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
