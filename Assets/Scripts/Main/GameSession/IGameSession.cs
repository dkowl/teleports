using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSession
{

    void Start(IGameState gameState);
    void End(System.Action onEndAction);
    IGameSessionResult GetResult();

    float TimeLeft { get; }

    //ugly stuff
    GameObject PlayerGameObject { get; }
}
