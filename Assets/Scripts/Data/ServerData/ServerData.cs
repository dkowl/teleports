using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData : IServerData {    

    [SerializeField]
    private UnitAttributeStats[] unitAttributeStats;

    public UnitAttributeStats GetAttributeStats(PlayerStats type)
    {
        foreach(UnitAttributeStats stat in unitAttributeStats)
        {
            if(stat.type == type)
            {
                return stat;
            }
        }
        return null;
    }
	
}
