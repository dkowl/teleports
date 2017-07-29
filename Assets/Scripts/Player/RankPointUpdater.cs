using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RankPointUpdater {

    private static readonly int ProbeLength = 5;

    public static int UpdateRankPoints(int current, int newScore)
    {
        return current + (newScore - current) / ProbeLength;
    }

	
}
