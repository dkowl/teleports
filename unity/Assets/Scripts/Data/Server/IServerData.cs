using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerData
{
    UnitAttributeStats GetAttributeStats(PlayerStats type);
}
