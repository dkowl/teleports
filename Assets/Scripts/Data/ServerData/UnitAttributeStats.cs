using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitAttributeStats
{
    public PlayerStats type;
    public float minLvlAvgVal, maxLvlAvgVal, standardDeviationPercentage;

    public float GetAttributeAvgValue(int level)
    {
        return Mathf.Lerp(minLvlAvgVal, maxLvlAvgVal, (float)level / XpLevels.MaxLevel);
    }

    public float GetSliderValue(int level, float value)
    {
        float avgValue = GetAttributeAvgValue(level);

        //0% - 50% --- interpolate between 0 and avg
        //50% - 100% --- interpolate between avg and avg + 3 * std_dev

        if(value <= avgValue)
        {
            return (value / avgValue) * 0.5f;
        }
        else
        {
            float maxValue = avgValue * (1 + (standardDeviationPercentage / 100) * 3);
            return (value - avgValue) / (maxValue - avgValue) * 0.5f + 0.5f;
        }
    }
}