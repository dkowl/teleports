using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistence
{
    IStaticData GetStaticData();
    IServerData GetServerData();
    void LoadGameState();
    void SaveGameState();
    IGameState GetGameState();
}
