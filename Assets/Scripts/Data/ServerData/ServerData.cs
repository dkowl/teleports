using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "serverData", menuName = "Custom/ServerData", order = 8)]
public class ServerData : ScriptableObject, IServerData {    

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
