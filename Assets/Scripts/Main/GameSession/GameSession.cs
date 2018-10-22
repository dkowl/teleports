using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameSession : IGameSession
{
    private IWorld world;
    private IGameState cachedGameState;

    public void Start(IGameState gameState)
    {
        cachedGameState = gameState;
        SpawnWorld();
        SpawnPlayer();
    }

    public IGameSessionResult GetResult()
    {
        var result = new GameSessionResult();
        return result;
    }

    private void SpawnWorld()
    {
        world = new World();
        world.Spawn(GenerateWorldCreationParams(cachedGameState));
    }

    private void SpawnPlayer()
    {

    }

    private static WorldCreationParams GenerateWorldCreationParams(IGameState gameState)
    {
        var result = new WorldCreationParams
        {
            seed = Random.Range(int.MinValue, int.MaxValue),
            worldData = Main.StaticData.Game.Worlds.RandomValue
        };
        return result;
    }

}
