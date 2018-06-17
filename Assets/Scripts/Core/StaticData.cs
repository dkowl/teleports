using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StaticData : IStaticData {

    private GameData gameData;
    private GraphicsData graphicsData;
    private ServerData serverData;
    private Stylesheet stylesheet;
    private DataDefaults defaults;

    public StaticData(
        GameData gameData, 
        GraphicsData graphicsData, 
        ServerData serverData,
        Stylesheet stylesheet, 
        DataDefaults defaults)
    {
        this.gameData = gameData;
        this.graphicsData = graphicsData;
        this.serverData = serverData;
        this.stylesheet = stylesheet;
        this.defaults = defaults;
    }

    public IGameData Game => gameData;
    public IGraphicsData Graphics => graphicsData;
    public IServerData Server => serverData;
    public Stylesheet Stylesheet => stylesheet;
    public DataDefaults Defaults => defaults;
}
