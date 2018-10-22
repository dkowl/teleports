using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSession
{

    void Start(IGameState gameState);
    IGameSessionResult GetResult();
}
