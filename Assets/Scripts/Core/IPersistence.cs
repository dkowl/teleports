using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistence
{
    IStaticData GetStaticData();
    IGameState LoadGameState();
    IServerData GetServerData();
}
