using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : IStaticData {

    private GameData gameData;
    private GraphicsData graphicsData;

    public StaticData(GameData gameData, GraphicsData graphicsData)
    {
        this.gameData = gameData;
        this.graphicsData = graphicsData;
    }

    public IGameData Game => gameData;
    public IGraphicsData Graphics => graphicsData;
}
