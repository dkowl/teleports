using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels {

    private static int[] xpLevels = new int[] {
        0,
        140,
        690,
        1890,
        3890,
        6990,
        11340,
        17610,
        26110,
        37310,
        52310,
        72310,
        97310,
        131310,
        175710,
        231610,
        304410,
        395810,
        507810,
        657810,
        847810,
        1082810,
        1382810,
        1762810,
        2212810,
        2812810,
        3552810,
        4445810,
        5605810,
        7045810
    };

    private static int[] rpLevels = new int[]
    {
        0,
        100,
        175,
        300,
        500,
        900,
        1500,
        2700,
        4600,
        8000,
        14000,
        25000
    };

    public static Levels xp = new Levels(xpLevels);
    public static Levels rp = new Levels(rpLevels);

    private int[] levels;   
    
    public Levels(int[] levels)
    {
        this.levels = levels;
    } 

    public int Level(int x)
    {
        int i = 1;
        for (; i < levels.Length; i++)
        {
            if (levels[i] > x)  return i;
        }
        return MaxLevel;
    }

    public int Current(int x)
    {
        int lvl = Level(x);
        if (lvl > 0 && lvl < MaxLevel)
            return x - levels[lvl - 1];
        else return 0;
    }
    
    public int Required(int x)
    {
        int lvl = Level(x);
        if (lvl > 0 && lvl < MaxLevel)
            return levels[lvl] - levels[lvl - 1];
        else return 1;
    }

    public int RemainingToNextLevel(int x)
    {
        return Required(x) - Current(x);
    }

    public int Owned(int x)
    {
        return levels[Level(x) - 1];
    }

    public float Progress(int x)
    {
        if (Level(x) == MaxLevel)
            return 1;
        else
            return (float)Current(x) / Required(x);
    }

    public List<Tuple<int, int>> GetSliderProgressionIntervals(int oldValue, int newValue)
    {
        Debug.Assert(oldValue <= newValue);
        var result = new List<Tuple<int, int>>();
        int oldLevel = Level(oldValue);
        int newLevel = Level(newValue);
        while (true)
        {
            if (oldLevel == newLevel)
            {
                result.Add(new Tuple<int, int>(oldValue, newValue));
                break;
            }
            else
            {
                int remainingToNextLevel = RemainingToNextLevel(oldValue);
                result.Add(new Tuple<int, int>(oldValue, oldValue + remainingToNextLevel));
                oldLevel++;
                oldValue += remainingToNextLevel;
            }
        }

        return result;
    }

    public int MaxLevel => levels.Length;

    public int MaxValue => levels[MaxLevel-1];
}
